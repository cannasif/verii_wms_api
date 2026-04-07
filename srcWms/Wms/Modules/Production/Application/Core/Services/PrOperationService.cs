using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrOperationService : IPrOperationService
{
    private readonly IRepository<PrOrder> _orders;
    private readonly IRepository<PrOperation> _operations;
    private readonly IRepository<PrOperationLine> _operationLines;
    private readonly IRepository<PrOperationEvent> _operationEvents;
    private readonly IRepository<PrOrderOutput> _orderOutputs;
    private readonly IRepository<PrOrderConsumption> _orderConsumptions;
    private readonly IRepository<PrHeader> _headers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public PrOperationService(
        IRepository<PrOrder> orders,
        IRepository<PrOperation> operations,
        IRepository<PrOperationLine> operationLines,
        IRepository<PrOperationEvent> operationEvents,
        IRepository<PrOrderOutput> orderOutputs,
        IRepository<PrOrderConsumption> orderConsumptions,
        IRepository<PrHeader> headers,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _orders = orders;
        _operations = operations;
        _operationLines = operationLines;
        _operationEvents = operationEvents;
        _orderOutputs = orderOutputs;
        _orderConsumptions = orderConsumptions;
        _headers = headers;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<PrOperationDto>> StartAsync(StartPrOperationRequestDto request, CancellationToken cancellationToken = default)
    {
        var order = await GetOrderAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return Error("PrOperationOrderNotFound", 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTimeProvider.Now;
            var operation = new PrOperation
            {
                BranchCode = order.BranchCode,
                OrderId = order.Id,
                OperationNo = $"OP-{order.Id}-{now:yyyyMMddHHmmss}",
                OperationType = string.IsNullOrWhiteSpace(request.OperationType) ? "ProductionRun" : request.OperationType.Trim(),
                Status = "Open",
                WorkcenterId = request.WorkcenterId,
                MachineId = request.MachineId,
                ExecutedByUserId = _currentUserAccessor.UserId,
                StartedAt = now,
                PlannedDurationMinutes = request.PlannedDurationMinutes,
                Description = NullIfWhiteSpace(request.Description),
                CreatedDate = now,
                CreatedBy = _currentUserAccessor.UserId
            };
            await _operations.AddAsync(operation, cancellationToken);

            await _operationEvents.AddAsync(new PrOperationEvent
            {
                BranchCode = order.BranchCode,
                Operation = operation,
                OrderId = order.Id,
                EventType = "Start",
                EventAt = now,
                PerformedByUserId = _currentUserAccessor.UserId,
                WorkcenterId = request.WorkcenterId,
                MachineId = request.MachineId,
                CreatedDate = now,
                CreatedBy = _currentUserAccessor.UserId
            }, cancellationToken);

            order.Status = "InProgress";
            order.ActualStartDate ??= now;
            order.SetUpdatedInfo(_currentUserAccessor.UserId);
            _orders.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SyncHeaderStatusAsync(order.HeaderId, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return await SuccessAsync(operation, order.OrderNo, "PrOperationStartedSuccessfully", cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PrOperationDto>> PauseAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ChangeOperationStateAsync(operationId, "Paused", "Pause", request, cancellationToken);
    }

    public async Task<ApiResponse<PrOperationDto>> ResumeAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ChangeOperationStateAsync(operationId, "Open", "Resume", request, cancellationToken);
    }

    public async Task<ApiResponse<PrOperationDto>> AddConsumptionAsync(long operationId, AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default)
    {
        return await AddOperationLineAsync(operationId, "Consumption", request, cancellationToken);
    }

    public async Task<ApiResponse<PrOperationDto>> AddOutputAsync(long operationId, AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default)
    {
        return await AddOperationLineAsync(operationId, "Output", request, cancellationToken);
    }

    public async Task<ApiResponse<PrOperationDto>> CompleteAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        var operation = await _operations.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == operationId && !x.IsDeleted, cancellationToken);
        if (operation == null)
        {
            return Error("PrOperationNotFound", 404);
        }

        var order = await GetOrderAsync(operation.OrderId, cancellationToken, tracking: true);
        if (order == null)
        {
            return Error("PrOperationOrderNotFound", 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTimeProvider.Now;
            operation.Status = "Completed";
            operation.CompletedAt = now;
            operation.ActualDurationMinutes = operation.StartedAt.HasValue ? (int)Math.Max(0, (now - operation.StartedAt.Value).TotalMinutes) : operation.ActualDurationMinutes;
            operation.NetWorkingDurationMinutes = Math.Max(0, (operation.ActualDurationMinutes ?? 0) - (operation.PauseDurationMinutes ?? 0));
            operation.SetUpdatedInfo(_currentUserAccessor.UserId);
            _operations.Update(operation);

            await _operationEvents.AddAsync(new PrOperationEvent
            {
                BranchCode = operation.BranchCode,
                OperationId = operation.Id,
                OrderId = order.Id,
                EventType = "Complete",
                EventReasonCode = NullIfWhiteSpace(request.ReasonCode),
                EventNote = NullIfWhiteSpace(request.Note),
                EventAt = now,
                DurationMinutes = request.DurationMinutes,
                PerformedByUserId = _currentUserAccessor.UserId,
                WorkcenterId = request.WorkcenterId ?? operation.WorkcenterId,
                MachineId = request.MachineId ?? operation.MachineId,
                CreatedDate = now,
                CreatedBy = _currentUserAccessor.UserId
            }, cancellationToken);

            if ((order.CompletedQuantity ?? 0m) >= order.PlannedQuantity)
            {
                order.Status = "Completed";
                order.ActualEndDate = now;
            }
            else
            {
                order.Status = "InProgress";
            }

            order.SetUpdatedInfo(_currentUserAccessor.UserId);
            _orders.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SyncHeaderStatusAsync(order.HeaderId, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return await SuccessAsync(operation, order.OrderNo, "PrOperationCompletedSuccessfully", cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ApiResponse<PrOperationDto>> ChangeOperationStateAsync(long operationId, string nextStatus, string eventType, PrOperationEventRequestDto request, CancellationToken cancellationToken)
    {
        var operation = await _operations.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == operationId && !x.IsDeleted, cancellationToken);
        if (operation == null)
        {
            return Error("PrOperationNotFound", 404);
        }

        var order = await GetOrderAsync(operation.OrderId, cancellationToken, tracking: true);
        if (order == null)
        {
            return Error("PrOperationOrderNotFound", 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTimeProvider.Now;
            operation.Status = nextStatus;
            if (eventType == "Pause" && request.DurationMinutes.HasValue)
            {
                operation.PauseDurationMinutes = (operation.PauseDurationMinutes ?? 0) + Math.Max(0, request.DurationMinutes.Value);
            }

            operation.SetUpdatedInfo(_currentUserAccessor.UserId);
            _operations.Update(operation);

            await _operationEvents.AddAsync(new PrOperationEvent
            {
                BranchCode = operation.BranchCode,
                OperationId = operation.Id,
                OrderId = order.Id,
                EventType = eventType,
                EventReasonCode = NullIfWhiteSpace(request.ReasonCode),
                EventNote = NullIfWhiteSpace(request.Note),
                EventAt = now,
                DurationMinutes = request.DurationMinutes,
                PerformedByUserId = _currentUserAccessor.UserId,
                WorkcenterId = request.WorkcenterId ?? operation.WorkcenterId,
                MachineId = request.MachineId ?? operation.MachineId,
                CreatedDate = now,
                CreatedBy = _currentUserAccessor.UserId
            }, cancellationToken);

            order.Status = eventType == "Pause" ? "Paused" : "InProgress";
            order.SetUpdatedInfo(_currentUserAccessor.UserId);
            _orders.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SyncHeaderStatusAsync(order.HeaderId, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return await SuccessAsync(operation, order.OrderNo, eventType == "Pause" ? "PrOperationPausedSuccessfully" : "PrOperationResumedSuccessfully", cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<ApiResponse<PrOperationDto>> AddOperationLineAsync(long operationId, string lineRole, AddPrOperationLineRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.StockCode) || request.Quantity <= 0)
        {
            return Error("PrOperationLineInvalid", 400);
        }

        var operation = await _operations.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == operationId && !x.IsDeleted, cancellationToken);
        if (operation == null)
        {
            return Error("PrOperationNotFound", 404);
        }

        var order = await GetOrderAsync(operation.OrderId, cancellationToken, tracking: true);
        if (order == null)
        {
            return Error("PrOperationOrderNotFound", 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var line = new PrOperationLine
            {
                BranchCode = order.BranchCode,
                OperationId = operation.Id,
                OrderId = order.Id,
                LineRole = lineRole,
                StockCode = request.StockCode.Trim(),
                YapKod = NullIfWhiteSpace(request.YapKod),
                Quantity = request.Quantity,
                Unit = NullIfWhiteSpace(request.Unit),
                SerialNo1 = NullIfWhiteSpace(request.SerialNo1),
                SerialNo2 = NullIfWhiteSpace(request.SerialNo2),
                SerialNo3 = NullIfWhiteSpace(request.SerialNo3),
                SerialNo4 = NullIfWhiteSpace(request.SerialNo4),
                LotNo = NullIfWhiteSpace(request.LotNo),
                BatchNo = NullIfWhiteSpace(request.BatchNo),
                SourceWarehouseCode = NullIfWhiteSpace(request.SourceWarehouseCode),
                TargetWarehouseCode = NullIfWhiteSpace(request.TargetWarehouseCode),
                SourceCellCode = NullIfWhiteSpace(request.SourceCellCode),
                TargetCellCode = NullIfWhiteSpace(request.TargetCellCode),
                ScannedBarcode = NullIfWhiteSpace(request.ScannedBarcode),
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = _currentUserAccessor.UserId
            };

            await _entityReferenceResolver.ResolveAsync(line, cancellationToken);

            if (lineRole == "Consumption")
            {
                var consumption = await _orderConsumptions.Query(tracking: true)
                    .Where(x => x.OrderId == order.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (consumption == null)
                {
                    return await RollbackErrorAsync("PrOperationConsumptionNotFound", 404, cancellationToken);
                }

                line.OrderConsumptionId = consumption.Id;
                consumption.ConsumedQuantity = (consumption.ConsumedQuantity ?? 0m) + line.Quantity;
                consumption.Status = (consumption.ConsumedQuantity ?? 0m) >= consumption.PlannedQuantity ? "Completed" : "InProgress";
                consumption.SetUpdatedInfo(_currentUserAccessor.UserId);
                _orderConsumptions.Update(consumption);
            }
            else
            {
                var output = await _orderOutputs.Query(tracking: true)
                    .Where(x => x.OrderId == order.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (output == null)
                {
                    return await RollbackErrorAsync("PrOperationOutputNotFound", 404, cancellationToken);
                }

                line.OrderOutputId = output.Id;
                output.ProducedQuantity = (output.ProducedQuantity ?? 0m) + line.Quantity;
                output.Status = (output.ProducedQuantity ?? 0m) >= output.PlannedQuantity ? "Completed" : "InProgress";
                output.SetUpdatedInfo(_currentUserAccessor.UserId);
                _orderOutputs.Update(output);

                order.StartedQuantity = (order.StartedQuantity ?? 0m) + line.Quantity;
                order.CompletedQuantity = (order.CompletedQuantity ?? 0m) + line.Quantity;
            }

            await _operationLines.AddAsync(line, cancellationToken);

            order.Status = "InProgress";
            order.SetUpdatedInfo(_currentUserAccessor.UserId);
            _orders.Update(order);

            operation.SetUpdatedInfo(_currentUserAccessor.UserId);
            _operations.Update(operation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await SyncHeaderStatusAsync(order.HeaderId, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return await SuccessAsync(operation, order.OrderNo, lineRole == "Consumption" ? "PrOperationConsumptionAddedSuccessfully" : "PrOperationOutputAddedSuccessfully", cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<PrOrder?> GetOrderAsync(long orderId, CancellationToken cancellationToken, bool tracking = false)
    {
        return await _orders.Query(tracking: tracking).FirstOrDefaultAsync(x => x.Id == orderId && !x.IsDeleted, cancellationToken);
    }

    private async Task SyncHeaderStatusAsync(long headerId, CancellationToken cancellationToken)
    {
        var header = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == headerId && !x.IsDeleted, cancellationToken);
        if (header == null)
        {
            return;
        }

        var orderStatuses = await _orders.Query()
            .Where(x => x.HeaderId == headerId && !x.IsDeleted)
            .Select(x => x.Status)
            .ToListAsync(cancellationToken);

        if (orderStatuses.Count == 0)
        {
            return;
        }

        if (orderStatuses.All(x => x == "Completed"))
        {
            header.Status = "Completed";
            header.ActualEndDate = DateTimeProvider.Now;
            header.CompletedQuantity = await _orders.Query()
                .Where(x => x.HeaderId == headerId && !x.IsDeleted)
                .SumAsync(x => (decimal?)x.CompletedQuantity, cancellationToken) ?? 0m;
            header.MarkAsCompleted();
        }
        else if (orderStatuses.Any(x => x == "InProgress" || x == "Paused"))
        {
            header.Status = "InProgress";
            header.ActualStartDate ??= DateTimeProvider.Now;
        }

        header.SetUpdatedInfo(_currentUserAccessor.UserId);
        _headers.Update(header);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<ApiResponse<PrOperationDto>> SuccessAsync(PrOperation operation, string? orderNo, string messageKey, CancellationToken cancellationToken)
    {
        var events = await _operationEvents.Query()
            .Where(x => x.OperationId == operation.Id && !x.IsDeleted)
            .OrderByDescending(x => x.EventAt)
            .Select(x => new PrOperationEventDto
            {
                Id = x.Id,
                EventType = x.EventType,
                EventReasonCode = x.EventReasonCode,
                EventNote = x.EventNote,
                EventAt = x.EventAt,
                DurationMinutes = x.DurationMinutes,
                PerformedByUserId = x.PerformedByUserId,
                WorkcenterId = x.WorkcenterId,
                MachineId = x.MachineId
            })
            .ToListAsync(cancellationToken);

        var lines = await _operationLines.Query()
            .Where(x => x.OperationId == operation.Id && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new PrOperationLineDto
            {
                Id = x.Id,
                LineRole = x.LineRole,
                StockCode = x.StockCode,
                YapKod = x.YapKod,
                Quantity = x.Quantity,
                Unit = x.Unit,
                SerialNo1 = x.SerialNo1,
                LotNo = x.LotNo,
                BatchNo = x.BatchNo,
                SourceWarehouseCode = x.SourceWarehouseCode,
                TargetWarehouseCode = x.TargetWarehouseCode,
                SourceCellCode = x.SourceCellCode,
                TargetCellCode = x.TargetCellCode,
                ScannedBarcode = x.ScannedBarcode,
                CreatedDate = x.CreatedDate ?? x.UpdatedDate ?? DateTime.MinValue
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<PrOperationDto>.SuccessResult(new PrOperationDto
        {
            Id = operation.Id,
            OrderId = operation.OrderId,
            OrderNo = orderNo,
            OperationNo = operation.OperationNo,
            OperationType = operation.OperationType,
            Status = operation.Status,
            StartedAt = operation.StartedAt,
            CompletedAt = operation.CompletedAt,
            PlannedDurationMinutes = operation.PlannedDurationMinutes,
            ActualDurationMinutes = operation.ActualDurationMinutes,
            PauseDurationMinutes = operation.PauseDurationMinutes,
            NetWorkingDurationMinutes = operation.NetWorkingDurationMinutes,
            Description = operation.Description,
            Events = events,
            Lines = lines
        }, _localizationService.GetLocalizedString(messageKey));
    }

    private ApiResponse<PrOperationDto> Error(string messageKey, int statusCode)
    {
        var message = _localizationService.GetLocalizedString(messageKey);
        return ApiResponse<PrOperationDto>.ErrorResult(message, message, statusCode);
    }

    private async Task<ApiResponse<PrOperationDto>> RollbackErrorAsync(string messageKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        return Error(messageKey, statusCode);
    }

    private static string? NullIfWhiteSpace(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

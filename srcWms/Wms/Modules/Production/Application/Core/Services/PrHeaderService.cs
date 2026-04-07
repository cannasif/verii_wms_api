using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Application.Production.Dtos;
using Wms.Application.System.Services;
using Wms.Domain.Common;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Communications.Enums;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Domain.Entities.Production;
using StockEntity = Wms.Domain.Entities.Stock.Stock;

namespace Wms.Application.Production.Services;

public sealed class PrHeaderService : IPrHeaderService
{
    private readonly IRepository<PrHeader> _headers;
    private readonly IRepository<PrLine> _lines;
    private readonly IRepository<PrImportLine> _importLines;
    private readonly IRepository<PrLineSerial> _lineSerials;
    private readonly IRepository<PrRoute> _routes;
    private readonly IRepository<PrTerminalLine> _terminalLines;
    private readonly IRepository<PrHeaderAssignment> _headerAssignments;
    private readonly IRepository<PrOrder> _orders;
    private readonly IRepository<PrOrderDependency> _orderDependencies;
    private readonly IRepository<PrOrderAssignment> _orderAssignments;
    private readonly IRepository<PrOrderOutput> _orderOutputs;
    private readonly IRepository<PrOrderConsumption> _orderConsumptions;
    private readonly IRepository<PrOperation> _operations;
    private readonly IRepository<PrParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly INotificationService _notificationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;
    private readonly IErpService _erpService;

    public PrHeaderService(
        IRepository<PrHeader> headers,
        IRepository<PrLine> lines,
        IRepository<PrImportLine> importLines,
        IRepository<PrLineSerial> lineSerials,
        IRepository<PrRoute> routes,
        IRepository<PrTerminalLine> terminalLines,
        IRepository<PrHeaderAssignment> headerAssignments,
        IRepository<PrOrder> orders,
        IRepository<PrOrderDependency> orderDependencies,
        IRepository<PrOrderAssignment> orderAssignments,
        IRepository<PrOrderOutput> orderOutputs,
        IRepository<PrOrderConsumption> orderConsumptions,
        IRepository<PrOperation> operations,
        IRepository<PrParameter> parameters,
        IRepository<Notification> notifications,
        IRepository<PHeader> packageHeaders,
        IRepository<StockEntity> stocks,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor,
        INotificationService notificationService,
        IEntityReferenceResolver entityReferenceResolver,
        IErpService erpService)
    {
        _headers = headers;
        _lines = lines;
        _importLines = importLines;
        _lineSerials = lineSerials;
        _routes = routes;
        _terminalLines = terminalLines;
        _headerAssignments = headerAssignments;
        _orders = orders;
        _orderDependencies = orderDependencies;
        _orderAssignments = orderAssignments;
        _orderOutputs = orderOutputs;
        _orderConsumptions = orderConsumptions;
        _operations = operations;
        _parameters = parameters;
        _notifications = notifications;
        _packageHeaders = packageHeaders;
        _stocks = stocks;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
        _notificationService = notificationService;
        _entityReferenceResolver = entityReferenceResolver;
        _erpService = erpService;
    }

    public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var entities = await _headers.Query().Where(x => x.BranchCode == branchCode && !x.IsDeleted).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrHeaderDto>>(entities);
        await PopulateDeleteEligibilityAsync(entities, dtos, cancellationToken);
        return ApiResponse<IEnumerable<PrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var branchCode = _currentUserAccessor.BranchCode ?? "0";

        var query = _headers.Query()
            .Where(x => x.BranchCode == branchCode && !x.IsDeleted)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);
        return ApiResponse<PagedResponse<PrHeaderDto>>.SuccessResult(new PagedResponse<PrHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<PrHeaderDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PrHeaderDto>(entity);
        var deleteState = await EvaluateDeleteStateAsync(entity, cancellationToken);
        dto.CanDelete = deleteState.CanDelete;
        dto.DeleteBlockedReason = deleteState.Reason;
        return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDetailDto>> GetDetailAsync(long id, CancellationToken cancellationToken = default)
    {
        var header = await _headers.Query().Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
        if (header == null)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<PrHeaderDetailDto>.ErrorResult(msg, msg, 404);
        }

        var headerAssignments = await _headerAssignments.Query()
            .Where(x => x.HeaderId == id && !x.IsDeleted)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var orders = await _orders.Query()
            .Where(x => x.HeaderId == id && !x.IsDeleted)
            .OrderBy(x => x.SequenceNo ?? int.MaxValue)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var orderIds = orders.Select(x => x.Id).ToList();
        var orderAssignments = orderIds.Count == 0
            ? new List<PrOrderAssignment>()
            : await _orderAssignments.Query().Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted).ToListAsync(cancellationToken);
        var outputs = orderIds.Count == 0
            ? new List<PrOrderOutput>()
            : await _orderOutputs.Query().Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted).ToListAsync(cancellationToken);
        var consumptions = orderIds.Count == 0
            ? new List<PrOrderConsumption>()
            : await _orderConsumptions.Query().Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted).ToListAsync(cancellationToken);
        var dependencies = orderIds.Count == 0
            ? new List<PrOrderDependency>()
            : await _orderDependencies.Query().Where(x => x.HeaderId == id && !x.IsDeleted).ToListAsync(cancellationToken);

        var orderNoById = orders.ToDictionary(x => x.Id, x => x.OrderNo);

        var detail = new PrHeaderDetailDto
        {
            Header = _mapper.Map<PrHeaderDto>(header),
            HeaderAssignments = headerAssignments.Select(x => new PrHeaderAssignmentItemDto
            {
                Id = x.Id,
                AssignedUserId = x.AssignedUserId,
                AssignedRoleId = x.AssignedRoleId,
                AssignedTeamId = x.AssignedTeamId,
                AssignmentType = x.AssignmentType,
                AssignedAt = x.AssignedAt,
                IsActive = x.IsActive
            }).ToList(),
            Orders = orders.Select(order => new PrOrderDetailDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                OrderType = order.OrderType,
                Status = order.Status,
                Priority = order.Priority,
                SequenceNo = order.SequenceNo,
                ParallelGroupNo = order.ParallelGroupNo,
                ProducedStockCode = order.ProducedStockCode,
                ProducedYapKod = order.ProducedYapKod,
                PlannedQuantity = order.PlannedQuantity,
                StartedQuantity = order.StartedQuantity,
                CompletedQuantity = order.CompletedQuantity,
                ScrapQuantity = order.ScrapQuantity,
                SourceWarehouseCode = order.SourceWarehouseCode,
                TargetWarehouseCode = order.TargetWarehouseCode,
                CanStartManually = order.CanStartManually,
                AutoStartWhenDependenciesDone = order.AutoStartWhenDependenciesDone,
                ActualStartDate = order.ActualStartDate,
                ActualEndDate = order.ActualEndDate,
                Assignments = orderAssignments.Where(x => x.OrderId == order.Id).Select(x => new PrOrderAssignmentItemDto
                {
                    Id = x.Id,
                    AssignedUserId = x.AssignedUserId,
                    AssignedRoleId = x.AssignedRoleId,
                    AssignedTeamId = x.AssignedTeamId,
                    AssignmentType = x.AssignmentType,
                    Note = x.Note,
                    IsActive = x.IsActive
                }).ToList(),
                Outputs = outputs.Where(x => x.OrderId == order.Id).Select(x => new PrOrderOutputItemDto
                {
                    Id = x.Id,
                    StockCode = x.StockCode,
                    YapKod = x.YapKod,
                    PlannedQuantity = x.PlannedQuantity,
                    ProducedQuantity = x.ProducedQuantity,
                    Unit = x.Unit,
                    TrackingMode = x.TrackingMode,
                    SerialEntryMode = x.SerialEntryMode,
                    TargetWarehouseCode = x.TargetWarehouseCode,
                    TargetCellCode = x.TargetCellCode,
                    Status = x.Status
                }).ToList(),
                Consumptions = consumptions.Where(x => x.OrderId == order.Id).Select(x => new PrOrderConsumptionItemDto
                {
                    Id = x.Id,
                    StockCode = x.StockCode,
                    YapKod = x.YapKod,
                    PlannedQuantity = x.PlannedQuantity,
                    ConsumedQuantity = x.ConsumedQuantity,
                    Unit = x.Unit,
                    TrackingMode = x.TrackingMode,
                    SerialEntryMode = x.SerialEntryMode,
                    SourceWarehouseCode = x.SourceWarehouseCode,
                    SourceCellCode = x.SourceCellCode,
                    IsBackflush = x.IsBackflush,
                    IsMandatory = x.IsMandatory,
                    Status = x.Status
                }).ToList()
            }).ToList(),
            Dependencies = dependencies.Select(x => new PrDependencyDetailDto
            {
                Id = x.Id,
                PredecessorOrderId = x.PredecessorOrderId,
                PredecessorOrderNo = orderNoById.GetValueOrDefault(x.PredecessorOrderId, string.Empty),
                SuccessorOrderId = x.SuccessorOrderId,
                SuccessorOrderNo = orderNoById.GetValueOrDefault(x.SuccessorOrderId, string.Empty),
                DependencyType = x.DependencyType,
                RequiredTransferCompleted = x.RequiredTransferCompleted,
                RequiredOutputAvailable = x.RequiredOutputAvailable,
                LagMinutes = x.LagMinutes
            }).ToList()
        };

        var headerDeleteState = await EvaluateDeleteStateAsync(header, cancellationToken);
        detail.Header.CanDelete = headerDeleteState.CanDelete;
        detail.Header.DeleteBlockedReason = headerDeleteState.Reason;

        return ApiResponse<PrHeaderDetailDto>.SuccessResult(detail, _localizationService.GetLocalizedString("PrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDto>> CreateAsync(CreatePrHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PrHeader>(createDto) ?? new PrHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.BranchCode = string.IsNullOrWhiteSpace(createDto.BranchCode) ? (_currentUserAccessor.BranchCode ?? "0") : createDto.BranchCode;
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PrHeaderDto>(entity);
        return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDto>> UpdateAsync(long id, UpdatePrHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<PrHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PrHeaderDto>(entity);
        return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (!string.Equals(entity.Status, "Draft", StringComparison.OrdinalIgnoreCase))
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderDeleteOnlyDraft");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var hasOperations = await _orders.Query()
            .Where(x => x.HeaderId == id && !x.IsDeleted)
            .Join(
                _operations.Query(),
                order => order.Id,
                operation => operation.OrderId,
                (_, _) => 1)
            .AnyAsync(cancellationToken);
        if (hasOperations)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderOperationsExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        if (hasImportLines)
        {
            var msg = _localizationService.GetLocalizedString("PrHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
        }

        var parameter = await _parameters.Query().FirstOrDefaultAsync(cancellationToken);
        var validationError = await ValidateLineSerialVsRouteQuantitiesAsync(id, parameter, cancellationToken);
        if (validationError != null)
        {
            return validationError;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            entity.MarkAsCompleted();
            entity.SetPendingApproval(parameter?.RequireApprovalBeforeErp == true);
            _headers.Update(entity);

            var packageHeader = await _packageHeaders.Query(tracking: true)
                .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.PR)
                .FirstOrDefaultAsync(cancellationToken);
            if (packageHeader != null)
            {
                packageHeader.Status = PHeaderStatus.Shipped;
                _packageHeaders.Update(packageHeader);
            }

            Notification? notification = null;
            if (entity.CreatedBy.HasValue)
            {
                var orderNumber = entity.Id.ToString();
                notification = new Notification
                {
                    Title = _localizationService.GetLocalizedString("PrDoneNotificationTitle", orderNumber),
                    Message = _localizationService.GetLocalizedString("PrDoneNotificationMessage", orderNumber),
                    TitleKey = "PrDoneNotificationTitle",
                    MessageKey = "PrDoneNotificationMessage",
                    Channel = NotificationChannel.Web,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = entity.CreatedBy.Value,
                    RelatedEntityType = NotificationEntityType.PRDone,
                    RelatedEntityId = entity.Id,
                    DeliveredAt = DateTimeProvider.Now
                };

                await _notifications.AddAsync(notification, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (notification != null)
            {
                await _notificationService.PublishSignalRNotificationsAsync(new[] { notification }, cancellationToken);
            }

            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<PrHeaderDto>>> GetAssignedProductionOrdersAsync(long userId, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var entities = await _headers.Query()
            .Where(h => !h.IsCompleted && !h.IsDeleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PrHeaderDto>>(entities);
        await PopulateDeleteEligibilityAsync(entities, dtos, cancellationToken);
        return ApiResponse<IEnumerable<PrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetAssignedProductionOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var query = _headers.Query()
            .Where(h => !h.IsCompleted && !h.IsDeleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);
        return ApiResponse<PagedResponse<PrHeaderDto>>.SuccessResult(new PagedResponse<PrHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PrHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrAssignedProductionOrderLinesDto>> GetAssignedProductionOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var lines = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var lineIds = lines.Select(x => x.Id).ToList();

        IEnumerable<PrLineSerial> lineSerials = Array.Empty<PrLineSerial>();
        if (lineIds.Count > 0)
        {
            lineSerials = await _lineSerials.Query().Where(x => lineIds.Contains(x.LineId) && !x.IsDeleted).ToListAsync(cancellationToken);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId && !x.IsDeleted).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();

        IEnumerable<PrRoute> routes = Array.Empty<PrRoute>();
        if (importLineIds.Count > 0)
        {
            routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted).ToListAsync(cancellationToken);
        }

        var lineDtos = _mapper.Map<List<PrLineDto>>(lines);
        if (lineDtos.Count > 0)
        {
        }
        var importLineDtos = _mapper.Map<List<PrImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
        }

        var dto = new PrAssignedProductionOrderLinesDto
        {
            Lines = lineDtos,
            LineSerials = _mapper.Map<List<PrLineSerialDto>>(lineSerials),
            ImportLines = importLineDtos,
            Routes = _mapper.Map<List<PrRouteDto>>(routes)
        };

        return ApiResponse<PrAssignedProductionOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderAssignedOrderLinesRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDto>> CreatePlanAsync(CreateProductionPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null || request.Header == null || request.Orders.Count == 0)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanRequestInvalid");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 400);
        }

        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var currentUserId = _currentUserAccessor.UserId;
        var documentNo = request.Header.DocumentNo?.Trim();
        if (string.IsNullOrWhiteSpace(documentNo))
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanDocumentNoRequired");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 400);
        }

        var duplicateDocument = await _headers.Query().AnyAsync(x => x.BranchCode == branchCode && x.DocumentNo == documentNo && !x.IsDeleted, cancellationToken);
        if (duplicateDocument)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanDocumentNoAlreadyExists");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 409);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = new PrHeader
            {
                BranchCode = branchCode,
                DocumentNo = documentNo,
                DocumentDate = request.Header.DocumentDate ?? DateTimeProvider.Now,
                Description1 = NullIfWhiteSpace(request.Header.Description),
                DocumentType = "PR_PLAN",
                ProjectCode = NullIfWhiteSpace(request.Header.ProjectCode),
                PlannedDate = request.Header.PlannedStartDate,
                IsPlanned = request.Header.PlannedStartDate.HasValue || request.Header.PlannedEndDate.HasValue,
                PriorityLevel = ClampPriorityLevel(request.Header.Priority),
                Priority = request.Header.Priority,
                PlanType = string.IsNullOrWhiteSpace(request.Header.PlanType) ? "Production" : request.Header.PlanType.Trim(),
                ExecutionMode = string.IsNullOrWhiteSpace(request.Header.ExecutionMode) ? "Serial" : request.Header.ExecutionMode.Trim(),
                Status = "Draft",
                CustomerCode = NullIfWhiteSpace(request.Header.CustomerCode),
                MainStockCode = NullIfWhiteSpace(request.Header.MainStockCode),
                MainYapKod = NullIfWhiteSpace(request.Header.MainYapKod),
                PlannedQuantity = request.Header.PlannedQuantity,
                Quantity = request.Header.PlannedQuantity,
                PlannedStartDate = request.Header.PlannedStartDate,
                PlannedEndDate = request.Header.PlannedEndDate,
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            };

            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            if (header.MainStockId.HasValue)
            {
                header.StockId = header.MainStockId;
                header.StockCode = header.MainStockCode;
            }
            if (header.MainYapKodId.HasValue)
            {
                header.YapKodId = header.MainYapKodId;
                header.YapKod = header.MainYapKod;
            }

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.Header.Assignments.Count > 0)
            {
                var headerAssignments = request.Header.Assignments
                    .Where(x => x.AssignedUserId.HasValue || x.AssignedRoleId.HasValue || x.AssignedTeamId.HasValue)
                    .Select(x => new PrHeaderAssignment
                    {
                        BranchCode = branchCode,
                        HeaderId = header.Id,
                        AssignedUserId = x.AssignedUserId,
                        AssignedRoleId = x.AssignedRoleId,
                        AssignedTeamId = x.AssignedTeamId,
                        AssignmentType = string.IsNullOrWhiteSpace(x.AssignmentType) ? "Primary" : x.AssignmentType.Trim(),
                        AssignedAt = DateTimeProvider.Now,
                        IsActive = true,
                        CreatedDate = DateTimeProvider.Now,
                        CreatedBy = currentUserId
                    })
                    .ToList();

                if (headerAssignments.Count > 0)
                {
                    await _headerAssignments.AddRangeAsync(headerAssignments, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            var orderIdByLocalId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var orderByLocalId = new Dictionary<string, PrOrder>(StringComparer.OrdinalIgnoreCase);
            var orders = new List<PrOrder>(request.Orders.Count);

            foreach (var orderDto in request.Orders)
            {
                if (string.IsNullOrWhiteSpace(orderDto.LocalId) || string.IsNullOrWhiteSpace(orderDto.OrderNo) || string.IsNullOrWhiteSpace(orderDto.ProducedStockCode))
                {
                    return await RollbackPlanErrorAsync("PrHeaderPlanOrderInvalid", 400, cancellationToken);
                }

                var order = new PrOrder
                {
                    BranchCode = branchCode,
                    HeaderId = header.Id,
                    OrderNo = orderDto.OrderNo.Trim(),
                    OrderDate = header.DocumentDate,
                    Description = header.Description1,
                    OrderType = string.IsNullOrWhiteSpace(orderDto.OrderType) ? "Production" : orderDto.OrderType.Trim(),
                    Status = "Draft",
                    Priority = header.Priority,
                    SequenceNo = orderDto.SequenceNo,
                    ParallelGroupNo = orderDto.ParallelGroupNo,
                    ProducedStockCode = orderDto.ProducedStockCode.Trim(),
                    ProducedYapKod = NullIfWhiteSpace(orderDto.ProducedYapKod),
                    PlannedQuantity = orderDto.PlannedQuantity,
                    SourceWarehouseCode = NullIfWhiteSpace(orderDto.SourceWarehouseCode),
                    TargetWarehouseCode = NullIfWhiteSpace(orderDto.TargetWarehouseCode),
                    CanStartManually = orderDto.CanStartManually,
                    AutoStartWhenDependenciesDone = orderDto.AutoStartWhenDependenciesDone,
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                };

                await _entityReferenceResolver.ResolveAsync(order, cancellationToken);
                orders.Add(order);
                orderByLocalId[orderDto.LocalId] = order;
            }

            await _orders.AddRangeAsync(orders, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var item in orderByLocalId)
            {
                orderIdByLocalId[item.Key] = item.Value.Id;
            }

            var orderAssignments = new List<PrOrderAssignment>();
            foreach (var orderDto in request.Orders)
            {
                if (!orderByLocalId.TryGetValue(orderDto.LocalId, out var order))
                {
                    continue;
                }

                orderAssignments.AddRange(orderDto.Assignments
                    .Where(x => x.AssignedUserId.HasValue || x.AssignedRoleId.HasValue || x.AssignedTeamId.HasValue)
                    .Select(x => new PrOrderAssignment
                    {
                        BranchCode = branchCode,
                        OrderId = order.Id,
                        AssignedUserId = x.AssignedUserId,
                        AssignedRoleId = x.AssignedRoleId,
                        AssignedTeamId = x.AssignedTeamId,
                        AssignmentType = string.IsNullOrWhiteSpace(x.AssignmentType) ? "Primary" : x.AssignmentType.Trim(),
                        AssignedAt = DateTimeProvider.Now,
                        IsActive = true,
                        Note = NullIfWhiteSpace(x.Note),
                        CreatedDate = DateTimeProvider.Now,
                        CreatedBy = currentUserId
                    }));
            }

            if (orderAssignments.Count > 0)
            {
                await _orderAssignments.AddRangeAsync(orderAssignments, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var outputs = new List<PrOrderOutput>();
            foreach (var outputDto in request.Outputs)
            {
                if (!orderIdByLocalId.TryGetValue(outputDto.OrderLocalId, out var orderId))
                {
                    return await RollbackPlanErrorAsync("PrHeaderPlanOrderReferenceInvalid", 400, cancellationToken);
                }

                var output = new PrOrderOutput
                {
                    BranchCode = branchCode,
                    OrderId = orderId,
                    StockCode = outputDto.StockCode.Trim(),
                    YapKod = NullIfWhiteSpace(outputDto.YapKod),
                    PlannedQuantity = outputDto.PlannedQuantity,
                    Unit = NullIfWhiteSpace(outputDto.Unit),
                    TrackingMode = string.IsNullOrWhiteSpace(outputDto.TrackingMode) ? "None" : outputDto.TrackingMode.Trim(),
                    SerialEntryMode = string.IsNullOrWhiteSpace(outputDto.SerialEntryMode) ? "Optional" : outputDto.SerialEntryMode.Trim(),
                    TargetWarehouseCode = NullIfWhiteSpace(outputDto.TargetWarehouseCode),
                    TargetCellCode = NullIfWhiteSpace(outputDto.TargetCellCode),
                    Status = "Draft",
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                };
                await _entityReferenceResolver.ResolveAsync(output, cancellationToken);
                outputs.Add(output);
            }

            if (outputs.Count == 0)
            {
                foreach (var order in orders)
                {
                    outputs.Add(new PrOrderOutput
                    {
                        BranchCode = branchCode,
                        OrderId = order.Id,
                        StockId = order.ProducedStockId,
                        StockCode = order.ProducedStockCode,
                        YapKodId = order.ProducedYapKodId,
                        YapKod = order.ProducedYapKod,
                        PlannedQuantity = order.PlannedQuantity,
                        TrackingMode = "None",
                        SerialEntryMode = "Optional",
                        TargetWarehouseId = order.TargetWarehouseId,
                        TargetWarehouseCode = order.TargetWarehouseCode,
                        Status = "Draft",
                        CreatedDate = DateTimeProvider.Now,
                        CreatedBy = currentUserId
                    });
                }
            }

            if (outputs.Count > 0)
            {
                await _orderOutputs.AddRangeAsync(outputs, cancellationToken);
            }

            var consumptions = new List<PrOrderConsumption>();
            foreach (var consumptionDto in request.Consumptions)
            {
                if (!orderIdByLocalId.TryGetValue(consumptionDto.OrderLocalId, out var orderId))
                {
                    return await RollbackPlanErrorAsync("PrHeaderPlanOrderReferenceInvalid", 400, cancellationToken);
                }

                var consumption = new PrOrderConsumption
                {
                    BranchCode = branchCode,
                    OrderId = orderId,
                    StockCode = consumptionDto.StockCode.Trim(),
                    YapKod = NullIfWhiteSpace(consumptionDto.YapKod),
                    PlannedQuantity = consumptionDto.PlannedQuantity,
                    Unit = NullIfWhiteSpace(consumptionDto.Unit),
                    TrackingMode = string.IsNullOrWhiteSpace(consumptionDto.TrackingMode) ? "None" : consumptionDto.TrackingMode.Trim(),
                    SerialEntryMode = string.IsNullOrWhiteSpace(consumptionDto.SerialEntryMode) ? "Optional" : consumptionDto.SerialEntryMode.Trim(),
                    SourceWarehouseCode = NullIfWhiteSpace(consumptionDto.SourceWarehouseCode),
                    SourceCellCode = NullIfWhiteSpace(consumptionDto.SourceCellCode),
                    IsBackflush = consumptionDto.IsBackflush,
                    IsMandatory = consumptionDto.IsMandatory,
                    Status = "Draft",
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                };
                await _entityReferenceResolver.ResolveAsync(consumption, cancellationToken);
                consumptions.Add(consumption);
            }

            if (consumptions.Count > 0)
            {
                await _orderConsumptions.AddRangeAsync(consumptions, cancellationToken);
            }

            var dependencies = new List<PrOrderDependency>();
            foreach (var dependencyDto in request.Dependencies)
            {
                if (!orderIdByLocalId.TryGetValue(dependencyDto.PredecessorOrderLocalId, out var predecessorOrderId)
                    || !orderIdByLocalId.TryGetValue(dependencyDto.SuccessorOrderLocalId, out var successorOrderId)
                    || predecessorOrderId == successorOrderId)
                {
                    return await RollbackPlanErrorAsync("PrHeaderPlanDependencyInvalid", 400, cancellationToken);
                }

                dependencies.Add(new PrOrderDependency
                {
                    BranchCode = branchCode,
                    HeaderId = header.Id,
                    PredecessorOrderId = predecessorOrderId,
                    SuccessorOrderId = successorOrderId,
                    DependencyType = string.IsNullOrWhiteSpace(dependencyDto.DependencyType) ? "FinishToStart" : dependencyDto.DependencyType.Trim(),
                    IsRequired = true,
                    LagMinutes = dependencyDto.LagMinutes,
                    RequiredTransferCompleted = dependencyDto.RequiredTransferCompleted,
                    RequiredOutputAvailable = dependencyDto.RequiredOutputAvailable,
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                });
            }

            if (dependencies.Count > 0)
            {
                await _orderDependencies.AddRangeAsync(dependencies, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var dto = _mapper.Map<PrHeaderDto>(header);
            return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderPlanCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<ProductionPlanDraftDto>> GetErpTemplateAsync(ProductionErpTemplateRequestDto request, CancellationToken cancellationToken = default)
    {
        if ((string.IsNullOrWhiteSpace(request.StockCode) && string.IsNullOrWhiteSpace(request.OrderNo)) || request.Quantity <= 0)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderErpTemplateRequestInvalid");
            return ApiResponse<ProductionPlanDraftDto>.ErrorResult(message, message, 400);
        }

        var normalizedStockCode = request.StockCode?.Trim();
        var normalizedOrderNo = request.OrderNo?.Trim();
        var normalizedYapKod = request.YapKod?.Trim();

        var template = new ProductionPlanDraftDto
        {
            Source = "erp",
            Header = new ProductionHeaderDraftDto
            {
                DocumentNo = normalizedOrderNo ?? normalizedStockCode ?? string.Empty,
                DocumentDate = DateTimeProvider.Now,
                Description = "ERP template",
                ExecutionMode = "Serial",
                PlanType = "Production",
                Priority = 0,
                MainStockCode = normalizedStockCode ?? string.Empty,
                MainYapKod = normalizedYapKod ?? string.Empty,
                PlannedQuantity = request.Quantity
            }
        };

        var erpHeadersResponse = !string.IsNullOrWhiteSpace(normalizedOrderNo)
            ? await _erpService.GetProductHeaderAsync(normalizedOrderNo, cancellationToken)
            : ApiResponse<List<ProductHeaderDto>>.SuccessResult(new List<ProductHeaderDto>(), string.Empty);

        var erpLinesResponse = await _erpService.GetProductLinesAsync(
            normalizedOrderNo,
            null,
            normalizedStockCode,
            cancellationToken);

        var erpHeaders = erpHeadersResponse.Success && erpHeadersResponse.Data != null
            ? erpHeadersResponse.Data
            : new List<ProductHeaderDto>();

        var erpLines = erpLinesResponse.Success && erpLinesResponse.Data != null
            ? erpLinesResponse.Data
            : new List<ProductLineDto>();

        if (erpHeaders.Count == 0 && erpLines.Count == 0)
        {
            var mirrorStock = !string.IsNullOrWhiteSpace(normalizedStockCode)
                ? await _stocks.Query().Where(x => x.ErpStockCode == normalizedStockCode && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken)
                : null;
            if (mirrorStock == null)
            {
                var message = _localizationService.GetLocalizedString("PrHeaderErpTemplateNotFound");
                return ApiResponse<ProductionPlanDraftDto>.ErrorResult(message, message, 404);
            }

            template.Header.MainStockCode = mirrorStock.ErpStockCode ?? normalizedStockCode ?? string.Empty;
            template.Header.MainYapKod = normalizedYapKod ?? string.Empty;

            var localId = "order-1";
            template.Orders.Add(new ProductionOrderDraftDto
            {
                LocalId = localId,
                OrderNo = normalizedOrderNo ?? $"TPL-{mirrorStock.ErpStockCode}",
                OrderType = "Production",
                ProducedStockCode = mirrorStock.ErpStockCode ?? string.Empty,
                ProducedYapKod = normalizedYapKod ?? string.Empty,
                PlannedQuantity = request.Quantity
            });
            template.Outputs.Add(new ProductionOutputDraftDto
            {
                LocalId = "output-1",
                OrderLocalId = localId,
                StockCode = mirrorStock.ErpStockCode ?? string.Empty,
                YapKod = normalizedYapKod ?? string.Empty,
                PlannedQuantity = request.Quantity,
                Unit = "ADET",
                TrackingMode = "None",
                SerialEntryMode = "Optional"
            });

            return ApiResponse<ProductionPlanDraftDto>.SuccessResult(template, _localizationService.GetLocalizedString("PrHeaderErpTemplateRetrievedSuccessfully"));
        }

        var localIdsByOrderNo = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var orderIndex = 1;
        foreach (var headerItem in erpHeaders)
        {
            var orderNo = headerItem.IsEmriNo?.Trim();
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                continue;
            }

            var localId = $"order-{orderIndex++}";
            localIdsByOrderNo[orderNo] = localId;
            template.Orders.Add(new ProductionOrderDraftDto
            {
                LocalId = localId,
                OrderNo = orderNo,
                OrderType = "Production",
                ProducedStockCode = headerItem.StockCode?.Trim() ?? normalizedStockCode ?? string.Empty,
                ProducedYapKod = headerItem.YapKod?.Trim() ?? normalizedYapKod ?? string.Empty,
                PlannedQuantity = headerItem.Quantity ?? request.Quantity,
                CanStartManually = false,
                AutoStartWhenDependenciesDone = false
            });
            template.Outputs.Add(new ProductionOutputDraftDto
            {
                LocalId = $"output-{localId}",
                OrderLocalId = localId,
                StockCode = headerItem.StockCode?.Trim() ?? normalizedStockCode ?? string.Empty,
                YapKod = headerItem.YapKod?.Trim() ?? normalizedYapKod ?? string.Empty,
                PlannedQuantity = headerItem.Quantity ?? request.Quantity,
                Unit = "ADET",
                TrackingMode = "None",
                SerialEntryMode = "Optional"
            });
        }

        if (template.Orders.Count == 0)
        {
            var orderNo = normalizedOrderNo ?? normalizedStockCode ?? "ERP-TEMPLATE";
            localIdsByOrderNo[orderNo] = "order-1";
            template.Orders.Add(new ProductionOrderDraftDto
            {
                LocalId = "order-1",
                OrderNo = orderNo,
                OrderType = "Production",
                ProducedStockCode = normalizedStockCode ?? erpLines.FirstOrDefault()?.MamulKodu ?? string.Empty,
                ProducedYapKod = normalizedYapKod ?? string.Empty,
                PlannedQuantity = request.Quantity
            });
            template.Outputs.Add(new ProductionOutputDraftDto
            {
                LocalId = "output-1",
                OrderLocalId = "order-1",
                StockCode = normalizedStockCode ?? erpLines.FirstOrDefault()?.MamulKodu ?? string.Empty,
                YapKod = normalizedYapKod ?? string.Empty,
                PlannedQuantity = request.Quantity,
                Unit = "ADET",
                TrackingMode = "None",
                SerialEntryMode = "Optional"
            });
        }

        foreach (var line in erpLines)
        {
            var orderNo = line.IsEmriNo?.Trim();
            if (string.IsNullOrWhiteSpace(orderNo) || !localIdsByOrderNo.TryGetValue(orderNo, out var orderLocalId))
            {
                orderLocalId = template.Orders[0].LocalId;
            }

            var plannedQuantity = line.HesaplananToplamMiktar ?? ((line.BirimMiktar ?? 0m) * request.Quantity);
            if (plannedQuantity <= 0)
            {
                plannedQuantity = line.BirimMiktar ?? request.Quantity;
            }

            template.Consumptions.Add(new ProductionConsumptionDraftDto
            {
                LocalId = $"cons-{template.Consumptions.Count + 1}",
                OrderLocalId = orderLocalId,
                StockCode = line.HamKodu?.Trim() ?? string.Empty,
                YapKod = string.Empty,
                PlannedQuantity = plannedQuantity,
                Unit = "ADET",
                TrackingMode = "None",
                SerialEntryMode = "Optional",
                IsBackflush = false,
                IsMandatory = true
            });
        }

        foreach (var headerItem in erpHeaders)
        {
            var successorOrderNo = headerItem.IsEmriNo?.Trim();
            var predecessorOrderNo = headerItem.RefIsEmriNo?.Trim();
            if (string.IsNullOrWhiteSpace(predecessorOrderNo) || string.IsNullOrWhiteSpace(successorOrderNo))
            {
                continue;
            }

            if (!localIdsByOrderNo.TryGetValue(predecessorOrderNo, out var predecessorLocalId)
                || !localIdsByOrderNo.TryGetValue(successorOrderNo, out var successorLocalId))
            {
                continue;
            }

            template.Dependencies.Add(new ProductionDependencyDraftDto
            {
                LocalId = $"dep-{template.Dependencies.Count + 1}",
                PredecessorOrderLocalId = predecessorLocalId,
                SuccessorOrderLocalId = successorLocalId,
                DependencyType = "FinishToStart",
                RequiredOutputAvailable = true,
                RequiredTransferCompleted = false,
                LagMinutes = 0
            });
        }

        if (template.Header.MainStockCode.Length == 0)
        {
            template.Header.MainStockCode = template.Orders.FirstOrDefault()?.ProducedStockCode ?? normalizedStockCode ?? string.Empty;
        }
        if (template.Header.MainYapKod.Length == 0)
        {
            template.Header.MainYapKod = template.Orders.FirstOrDefault()?.ProducedYapKod ?? normalizedYapKod ?? string.Empty;
        }
        if (string.IsNullOrWhiteSpace(template.Header.DocumentNo))
        {
            template.Header.DocumentNo = normalizedOrderNo ?? template.Orders.FirstOrDefault()?.OrderNo ?? normalizedStockCode ?? string.Empty;
        }

        return ApiResponse<ProductionPlanDraftDto>.SuccessResult(template, _localizationService.GetLocalizedString("PrHeaderErpTemplateRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrHeaderDto>> GenerateProductionOrderAsync(GenerateProductionOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<PrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<PrHeader>(request.Header) ?? new PrHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();

            if (request.Lines?.Count > 0)
            {
                var lines = new List<PrLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<PrLine>(lineDto) ?? new PrLine();
                    await _entityReferenceResolver.ResolveAsync(line, cancellationToken);
                    line.HeaderId = header.Id;
                    lines.Add(line);
                }

                await _lines.AddRangeAsync(lines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                for (var i = 0; i < request.Lines.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(request.Lines[i].ClientKey))
                    {
                        lineKeyToId[request.Lines[i].ClientKey!] = lines[i].Id;
                    }

                    if (request.Lines[i].ClientGuid.HasValue)
                    {
                        lineGuidToId[request.Lines[i].ClientGuid!.Value] = lines[i].Id;
                    }
                }
            }

            if (request.LineSerials?.Count > 0)
            {
                var serials = new List<PrLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<PrLineSerial>(serialDto) ?? new PrLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var createdNotifications = new List<Notification>();
            if (request.TerminalLines?.Count > 0)
            {
                var terminalLines = request.TerminalLines
                    .Select(x =>
                    {
                        var terminalLine = _mapper.Map<PrTerminalLine>(x) ?? new PrTerminalLine();
                        terminalLine.HeaderId = header.Id;
                        return terminalLine;
                    })
                    .ToList();

                await _terminalLines.AddRangeAsync(terminalLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                    terminalLines,
                    header.Id.ToString(),
                    NotificationEntityType.PRHeader,
                    "PR_HEADER",
                    "PrHeaderNotificationTitle",
                    "PrHeaderNotificationMessage",
                    cancellationToken);

                if (createdNotifications.Count > 0)
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (createdNotifications.Count > 0)
            {
                await _notificationService.PublishSignalRNotificationsForCreatedNotificationsAsync(createdNotifications, cancellationToken);
            }

            var dto = _mapper.Map<PrHeaderDto>(header);
            return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PrHeaderDto>> UpdatePlanAsync(long id, CreateProductionPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null || request.Header == null || request.Orders.Count == 0)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanRequestInvalid");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 400);
        }

        var header = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (header == null)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 404);
        }

        if (!string.Equals(header.Status, "Draft", StringComparison.OrdinalIgnoreCase))
        {
            const string onlyDraftMessage = "Sadece draft durumundaki uretim planlari duzenlenebilir.";
            return ApiResponse<PrHeaderDto>.ErrorResult(onlyDraftMessage, onlyDraftMessage, 400);
        }

        var existingOrders = await _orders.Query().Where(x => x.HeaderId == id && !x.IsDeleted).ToListAsync(cancellationToken);
        var existingOrderIds = existingOrders.Select(x => x.Id).ToList();

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        var hasOperations = existingOrderIds.Count > 0
            && await _operations.Query().AnyAsync(x => existingOrderIds.Contains(x.OrderId) && !x.IsDeleted, cancellationToken);
        if (hasImportLines || hasOperations)
        {
            const string processedPlanMessage = "Islem gormus uretim planlari duzenlenemez.";
            return ApiResponse<PrHeaderDto>.ErrorResult(processedPlanMessage, processedPlanMessage, 400);
        }

        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var currentUserId = _currentUserAccessor.UserId;
        var documentNo = request.Header.DocumentNo?.Trim();
        if (string.IsNullOrWhiteSpace(documentNo))
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanDocumentNoRequired");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 400);
        }

        var duplicateDocument = await _headers.Query()
            .AnyAsync(x => x.BranchCode == branchCode && x.DocumentNo == documentNo && x.Id != id && !x.IsDeleted, cancellationToken);
        if (duplicateDocument)
        {
            var message = _localizationService.GetLocalizedString("PrHeaderPlanDocumentNoAlreadyExists");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 409);
        }

        var existingHeaderAssignmentIds = await _headerAssignments.Query().Where(x => x.HeaderId == id && !x.IsDeleted).Select(x => x.Id).ToListAsync(cancellationToken);
        var existingOrderAssignmentIds = existingOrderIds.Count == 0
            ? new List<long>()
            : await _orderAssignments.Query().Where(x => existingOrderIds.Contains(x.OrderId) && !x.IsDeleted).Select(x => x.Id).ToListAsync(cancellationToken);
        var existingOutputIds = existingOrderIds.Count == 0
            ? new List<long>()
            : await _orderOutputs.Query().Where(x => existingOrderIds.Contains(x.OrderId) && !x.IsDeleted).Select(x => x.Id).ToListAsync(cancellationToken);
        var existingConsumptionIds = existingOrderIds.Count == 0
            ? new List<long>()
            : await _orderConsumptions.Query().Where(x => existingOrderIds.Contains(x.OrderId) && !x.IsDeleted).Select(x => x.Id).ToListAsync(cancellationToken);
        var existingDependencyIds = await _orderDependencies.Query().Where(x => x.HeaderId == id && !x.IsDeleted).Select(x => x.Id).ToListAsync(cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            header.DocumentNo = documentNo;
            header.DocumentDate = request.Header.DocumentDate ?? header.DocumentDate ?? DateTimeProvider.Now;
            header.Description1 = NullIfWhiteSpace(request.Header.Description);
            header.ProjectCode = NullIfWhiteSpace(request.Header.ProjectCode);
            header.PlannedDate = request.Header.PlannedStartDate;
            header.IsPlanned = request.Header.PlannedStartDate.HasValue || request.Header.PlannedEndDate.HasValue;
            header.PriorityLevel = ClampPriorityLevel(request.Header.Priority);
            header.Priority = request.Header.Priority;
            header.PlanType = string.IsNullOrWhiteSpace(request.Header.PlanType) ? "Production" : request.Header.PlanType.Trim();
            header.ExecutionMode = string.IsNullOrWhiteSpace(request.Header.ExecutionMode) ? "Serial" : request.Header.ExecutionMode.Trim();
            header.CustomerCode = NullIfWhiteSpace(request.Header.CustomerCode);
            header.MainStockCode = NullIfWhiteSpace(request.Header.MainStockCode);
            header.MainYapKod = NullIfWhiteSpace(request.Header.MainYapKod);
            header.PlannedQuantity = request.Header.PlannedQuantity;
            header.Quantity = request.Header.PlannedQuantity;
            header.PlannedStartDate = request.Header.PlannedStartDate;
            header.PlannedEndDate = request.Header.PlannedEndDate;
            header.UpdatedDate = DateTimeProvider.Now;
            header.UpdatedBy = currentUserId;

            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            if (header.MainStockId.HasValue)
            {
                header.StockId = header.MainStockId;
                header.StockCode = header.MainStockCode;
            }
            if (header.MainYapKodId.HasValue)
            {
                header.YapKodId = header.MainYapKodId;
                header.YapKod = header.MainYapKod;
            }
            _headers.Update(header);

            if (existingDependencyIds.Count > 0) _orderDependencies.SoftDeleteRange(existingDependencyIds);
            if (existingOrderAssignmentIds.Count > 0) _orderAssignments.SoftDeleteRange(existingOrderAssignmentIds);
            if (existingOutputIds.Count > 0) _orderOutputs.SoftDeleteRange(existingOutputIds);
            if (existingConsumptionIds.Count > 0) _orderConsumptions.SoftDeleteRange(existingConsumptionIds);
            if (existingOrderIds.Count > 0) _orders.SoftDeleteRange(existingOrderIds);
            if (existingHeaderAssignmentIds.Count > 0) _headerAssignments.SoftDeleteRange(existingHeaderAssignmentIds);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var recreateResponse = await CreatePlanChildrenAsync(header, request, branchCode, currentUserId, cancellationToken);
            if (!recreateResponse.Success)
            {
                return recreateResponse;
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<PrHeaderDto>.SuccessResult(_mapper.Map<PrHeaderDto>(header), "Uretim plani guncellendi.");
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PrHeaderDto>> BulkPrGenerateAsync(BulkPrGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            return ApiResponse<PrHeaderDto>.ErrorResult(
                _localizationService.GetLocalizedString("InvalidModelState"),
                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var parameter = await _parameters.Query().FirstOrDefaultAsync(cancellationToken);
            var header = _mapper.Map<PrHeader>(request.Header) ?? new PrHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (header.Id <= 0)
            {
                return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderCreationError", "HeaderInsertFailed", 500, cancellationToken);
            }

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();
            if (request.Lines?.Count > 0)
            {
                var lines = new List<PrLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<PrLine>(lineDto) ?? new PrLine();
                    await _entityReferenceResolver.ResolveAsync(line, cancellationToken);
                    line.HeaderId = header.Id;
                    lines.Add(line);
                }

                await _lines.AddRangeAsync(lines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                for (var i = 0; i < request.Lines.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(request.Lines[i].ClientKey))
                    {
                        lineKeyToId[request.Lines[i].ClientKey!] = lines[i].Id;
                    }
                    if (request.Lines[i].ClientGuid.HasValue)
                    {
                        lineGuidToId[request.Lines[i].ClientGuid!.Value] = lines[i].Id;
                    }
                }
            }

            if (request.LineSerials?.Count > 0)
            {
                var serials = new List<PrLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "PrHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<PrLineSerial>(serialDto) ?? new PrLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.ImportLines?.Count > 0)
            {
                var importLines = new List<PrImportLine>(request.ImportLines.Count);
                foreach (var importDto in request.ImportLines)
                {
                    long? lineId = null;
                    if (importDto.LineGroupGuid.HasValue && lineGuidToId.TryGetValue(importDto.LineGroupGuid.Value, out var guidLineId))
                    {
                        lineId = guidLineId;
                    }
                    else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey) && lineKeyToId.TryGetValue(importDto.LineClientKey!, out var keyLineId))
                    {
                        lineId = keyLineId;
                    }

                    var importLine = new PrImportLine
                    {
                        HeaderId = header.Id,
                        LineId = lineId,
                        StockCode = importDto.StockCode,
                        StockId = importDto.StockId,
                        YapKod = importDto.YapKod,
                        YapKodId = importDto.YapKodId
                    };
                    await _entityReferenceResolver.ResolveAsync(importLine, cancellationToken);
                    importLines.Add(importLine);
                }

                await _importLines.AddRangeAsync(importLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                for (var i = 0; i < request.ImportLines.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(request.ImportLines[i].ClientKey))
                    {
                        importLineKeyToId[request.ImportLines[i].ClientKey!] = importLines[i].Id;
                    }
                    if (request.ImportLines[i].ClientGroupGuid.HasValue)
                    {
                        importLineKeyToId[request.ImportLines[i].ClientGroupGuid!.Value.ToString()] = importLines[i].Id;
                    }
                }
            }

            if (request.Routes?.Count > 0)
            {
                var routes = new List<PrRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    long importLineId;
                    if (routeDto.ImportLineGroupGuid.HasValue)
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineGroupGuid.Value.ToString(), out importLineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "ImportLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey!, out importLineId))
                        {
                            return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "ImportLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PrHeaderDto>("PrHeaderInvalidCorrelationKey", "ImportLineReferenceMissing", 400, cancellationToken);
                    }

                    var route = new PrRoute
                    {
                        ImportLineId = importLineId,
                        Quantity = routeDto.Quantity,
                        SerialNo = routeDto.SerialNo,
                        SerialNo2 = routeDto.SerialNo2,
                        SerialNo3 = routeDto.SerialNo3,
                        SerialNo4 = routeDto.SerialNo4,
                        ScannedBarcode = routeDto.ScannedBarcode ?? string.Empty,
                        SourceWarehouse = routeDto.SourceWarehouse,
                        TargetWarehouse = routeDto.TargetWarehouse,
                        SourceCellCode = routeDto.SourceCellCode,
                        TargetCellCode = routeDto.TargetCellCode
                    };
                    routes.Add(route);
                }

                await _routes.AddRangeAsync(routes, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            var dto = _mapper.Map<PrHeaderDto>(header);
            return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderBulkPrGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PagedResponse<PrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);

        var result = new PagedResponse<PrHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<PrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PrHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
    }

    private async Task PopulateDeleteEligibilityAsync(IReadOnlyCollection<PrHeader> entities, IReadOnlyCollection<PrHeaderDto> dtos, CancellationToken cancellationToken)
    {
        if (entities.Count == 0 || dtos.Count == 0)
        {
            return;
        }

        var headerIds = entities.Select(x => x.Id).ToList();
        var importLineHeaderIds = await _importLines.Query()
            .Where(x => headerIds.Contains(x.HeaderId) && !x.IsDeleted)
            .Select(x => x.HeaderId)
            .Distinct()
            .ToListAsync(cancellationToken);
        var orderRows = await _orders.Query()
            .Where(x => headerIds.Contains(x.HeaderId) && !x.IsDeleted)
            .Select(x => new { x.Id, x.HeaderId })
            .ToListAsync(cancellationToken);
        var orderIds = orderRows.Select(x => x.Id).ToList();
        var orderToHeader = orderRows.ToDictionary(x => x.Id, x => x.HeaderId);
        var operationOrderIds = orderIds.Count == 0
            ? new List<long>()
            : await _operations.Query()
                .Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted)
                .Select(x => x.OrderId)
                .Distinct()
                .ToListAsync(cancellationToken);
        var operationHeaderIds = operationOrderIds
            .Where(orderToHeader.ContainsKey)
            .Select(orderId => orderToHeader[orderId])
            .Distinct()
            .ToList();

        var importLineSet = importLineHeaderIds.ToHashSet();
        var operationSet = operationHeaderIds.ToHashSet();
        var dtoById = dtos.ToDictionary(x => x.Id);

        foreach (var entity in entities)
        {
            if (!dtoById.TryGetValue(entity.Id, out var dto))
            {
                continue;
            }

            var state = EvaluateDeleteState(entity, importLineSet.Contains(entity.Id), operationSet.Contains(entity.Id));
            dto.CanDelete = state.CanDelete;
            dto.DeleteBlockedReason = state.Reason;
        }
    }

    private async Task<(bool CanDelete, string? Reason)> EvaluateDeleteStateAsync(PrHeader entity, CancellationToken cancellationToken)
    {
        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == entity.Id && !x.IsDeleted, cancellationToken);
        var orderIds = await _orders.Query()
            .Where(x => x.HeaderId == entity.Id && !x.IsDeleted)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
        var hasOperations = orderIds.Count > 0
            && await _operations.Query().AnyAsync(x => orderIds.Contains(x.OrderId) && !x.IsDeleted, cancellationToken);
        return EvaluateDeleteState(entity, hasImportLines, hasOperations);
    }

    private (bool CanDelete, string? Reason) EvaluateDeleteState(PrHeader entity, bool hasImportLines, bool hasOperations)
    {
        if (entity.IsDeleted)
        {
            return (false, _localizationService.GetLocalizedString("PrHeaderNotFound"));
        }

        if (!string.Equals(entity.Status, "Draft", StringComparison.OrdinalIgnoreCase))
        {
            return (false, _localizationService.GetLocalizedString("PrHeaderDeleteOnlyDraft"));
        }

        if (hasOperations)
        {
            return (false, _localizationService.GetLocalizedString("PrHeaderOperationsExist"));
        }

        if (hasImportLines)
        {
            return (false, _localizationService.GetLocalizedString("PrHeaderImportLinesExist"));
        }

        return (true, null);
    }

    public async Task<ApiResponse<PrHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var notFound = _localizationService.GetLocalizedString("PrHeaderNotFound");
            return ApiResponse<PrHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
        {
            var message = _localizationService.GetLocalizedString("PrHeaderApprovalUpdateError");
            return ApiResponse<PrHeaderDto>.ErrorResult(message, message, 400);
        }

        var currentUserId = _currentUserAccessor.UserId ?? 0;
        if (approved)
        {
            entity.Approve(currentUserId);
        }
        else
        {
            entity.Reject(currentUserId);
        }

        entity.SetPendingApproval(false);
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PrHeaderDto>(entity);
        return ApiResponse<PrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderApprovalUpdatedSuccessfully"));
    }

    private async Task<ApiResponse<bool>?> ValidateLineSerialVsRouteQuantitiesAsync(long headerId, PrParameter? parameter, CancellationToken cancellationToken)
    {
        var skipValidation = (parameter?.AllowLessQuantityBasedOnOrder ?? false)
            && (parameter?.AllowMoreQuantityBasedOnOrder ?? false);

        var requireAllOrderItemsCollected = parameter?.RequireAllOrderItemsCollected ?? false;

        if (skipValidation)
        {
            return null;
        }

        var lines = await _lines.Query().Where(l => l.HeaderId == headerId).ToListAsync(cancellationToken);
        foreach (var line in lines)
        {
            var lineSerials = await _lineSerials.Query()
                .Where(ls => ls.LineId == line.Id)
                .ToListAsync(cancellationToken);

            var routes = await _routes.Query()
                .Where(r => r.ImportLine.LineId == line.Id && !r.ImportLine.IsDeleted)
                .ToListAsync(cancellationToken);

            var totalRouteQuantity = routes.Sum(r => r.Quantity);

            if (requireAllOrderItemsCollected)
            {
                if (totalRouteQuantity <= 0.000001m)
                {
                    var msg = _localizationService.GetLocalizedString("PrHeaderAllOrderItemsMustBeCollected", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty);
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
            }
            else if (totalRouteQuantity <= 0.000001m)
            {
                continue;
            }

            var allowLess = parameter?.AllowLessQuantityBasedOnOrder ?? false;
            var allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;
            var validationResult = LineSerialRouteValidationHelper.Validate(lineSerials, routes, allowLess, allowMore);
            if (!validationResult.HasMismatch)
            {
                continue;
            }

            var messageKey = validationResult.MismatchType switch
            {
                LineSerialRouteValidationMismatch.ExactMatchRequired => "PrHeaderQuantityExactMatchRequired",
                LineSerialRouteValidationMismatch.CannotBeGreater => "PrHeaderQuantityCannotBeGreater",
                LineSerialRouteValidationMismatch.CannotBeLess => "PrHeaderQuantityCannotBeLess",
                _ => "PrHeaderQuantityExactMatchRequired"
            };

            var validationMessage = _localizationService.GetLocalizedString(messageKey, line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, validationResult.ExpectedQuantity, validationResult.ActualQuantity);
            return ApiResponse<bool>.ErrorResult(validationMessage, validationMessage, 400);
        }

        return null;
    }

    private async Task<ApiResponse<PrHeaderDto>> CreatePlanChildrenAsync(
        PrHeader header,
        CreateProductionPlanRequestDto request,
        string branchCode,
        long? currentUserId,
        CancellationToken cancellationToken)
    {
        if (request.Header.Assignments.Count > 0)
        {
            var headerAssignments = request.Header.Assignments
                .Where(x => x.AssignedUserId.HasValue || x.AssignedRoleId.HasValue || x.AssignedTeamId.HasValue)
                .Select(x => new PrHeaderAssignment
                {
                    BranchCode = branchCode,
                    HeaderId = header.Id,
                    AssignedUserId = x.AssignedUserId,
                    AssignedRoleId = x.AssignedRoleId,
                    AssignedTeamId = x.AssignedTeamId,
                    AssignmentType = string.IsNullOrWhiteSpace(x.AssignmentType) ? "Primary" : x.AssignmentType.Trim(),
                    AssignedAt = DateTimeProvider.Now,
                    IsActive = true,
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                })
                .ToList();

            if (headerAssignments.Count > 0)
            {
                await _headerAssignments.AddRangeAsync(headerAssignments, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        var orderIdByLocalId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var orderByLocalId = new Dictionary<string, PrOrder>(StringComparer.OrdinalIgnoreCase);
        var orders = new List<PrOrder>(request.Orders.Count);

        foreach (var orderDto in request.Orders)
        {
            if (string.IsNullOrWhiteSpace(orderDto.LocalId) || string.IsNullOrWhiteSpace(orderDto.OrderNo) || string.IsNullOrWhiteSpace(orderDto.ProducedStockCode))
            {
                return await RollbackPlanErrorAsync("PrHeaderPlanOrderInvalid", 400, cancellationToken);
            }

            var order = new PrOrder
            {
                BranchCode = branchCode,
                HeaderId = header.Id,
                OrderNo = orderDto.OrderNo.Trim(),
                OrderDate = header.DocumentDate,
                Description = header.Description1,
                OrderType = string.IsNullOrWhiteSpace(orderDto.OrderType) ? "Production" : orderDto.OrderType.Trim(),
                Status = "Draft",
                Priority = header.Priority,
                SequenceNo = orderDto.SequenceNo,
                ParallelGroupNo = orderDto.ParallelGroupNo,
                ProducedStockCode = orderDto.ProducedStockCode.Trim(),
                ProducedYapKod = NullIfWhiteSpace(orderDto.ProducedYapKod),
                PlannedQuantity = orderDto.PlannedQuantity,
                SourceWarehouseCode = NullIfWhiteSpace(orderDto.SourceWarehouseCode),
                TargetWarehouseCode = NullIfWhiteSpace(orderDto.TargetWarehouseCode),
                CanStartManually = orderDto.CanStartManually,
                AutoStartWhenDependenciesDone = orderDto.AutoStartWhenDependenciesDone,
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            };

            await _entityReferenceResolver.ResolveAsync(order, cancellationToken);
            orders.Add(order);
            orderByLocalId[orderDto.LocalId] = order;
        }

        await _orders.AddRangeAsync(orders, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var item in orderByLocalId)
        {
            orderIdByLocalId[item.Key] = item.Value.Id;
        }

        var orderAssignments = new List<PrOrderAssignment>();
        foreach (var orderDto in request.Orders)
        {
            if (!orderByLocalId.TryGetValue(orderDto.LocalId, out var order))
            {
                continue;
            }

            orderAssignments.AddRange(orderDto.Assignments
                .Where(x => x.AssignedUserId.HasValue || x.AssignedRoleId.HasValue || x.AssignedTeamId.HasValue)
                .Select(x => new PrOrderAssignment
                {
                    BranchCode = branchCode,
                    OrderId = order.Id,
                    AssignedUserId = x.AssignedUserId,
                    AssignedRoleId = x.AssignedRoleId,
                    AssignedTeamId = x.AssignedTeamId,
                    AssignmentType = string.IsNullOrWhiteSpace(x.AssignmentType) ? "Primary" : x.AssignmentType.Trim(),
                    AssignedAt = DateTimeProvider.Now,
                    IsActive = true,
                    Note = NullIfWhiteSpace(x.Note),
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                }));
        }

        if (orderAssignments.Count > 0)
        {
            await _orderAssignments.AddRangeAsync(orderAssignments, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var outputs = new List<PrOrderOutput>();
        foreach (var outputDto in request.Outputs)
        {
            if (!orderIdByLocalId.TryGetValue(outputDto.OrderLocalId, out var orderId))
            {
                return await RollbackPlanErrorAsync("PrHeaderPlanOrderReferenceInvalid", 400, cancellationToken);
            }

            var output = new PrOrderOutput
            {
                BranchCode = branchCode,
                OrderId = orderId,
                StockCode = outputDto.StockCode.Trim(),
                YapKod = NullIfWhiteSpace(outputDto.YapKod),
                PlannedQuantity = outputDto.PlannedQuantity,
                Unit = NullIfWhiteSpace(outputDto.Unit),
                TrackingMode = string.IsNullOrWhiteSpace(outputDto.TrackingMode) ? "None" : outputDto.TrackingMode.Trim(),
                SerialEntryMode = string.IsNullOrWhiteSpace(outputDto.SerialEntryMode) ? "Optional" : outputDto.SerialEntryMode.Trim(),
                TargetWarehouseCode = NullIfWhiteSpace(outputDto.TargetWarehouseCode),
                TargetCellCode = NullIfWhiteSpace(outputDto.TargetCellCode),
                Status = "Draft",
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            };
            await _entityReferenceResolver.ResolveAsync(output, cancellationToken);
            outputs.Add(output);
        }

        if (outputs.Count == 0)
        {
            foreach (var order in orders)
            {
                outputs.Add(new PrOrderOutput
                {
                    BranchCode = branchCode,
                    OrderId = order.Id,
                    StockId = order.ProducedStockId,
                    StockCode = order.ProducedStockCode,
                    YapKodId = order.ProducedYapKodId,
                    YapKod = order.ProducedYapKod,
                    PlannedQuantity = order.PlannedQuantity,
                    TrackingMode = "None",
                    SerialEntryMode = "Optional",
                    TargetWarehouseId = order.TargetWarehouseId,
                    TargetWarehouseCode = order.TargetWarehouseCode,
                    Status = "Draft",
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                });
            }
        }

        if (outputs.Count > 0)
        {
            await _orderOutputs.AddRangeAsync(outputs, cancellationToken);
        }

        var consumptions = new List<PrOrderConsumption>();
        foreach (var consumptionDto in request.Consumptions)
        {
            if (!orderIdByLocalId.TryGetValue(consumptionDto.OrderLocalId, out var orderId))
            {
                return await RollbackPlanErrorAsync("PrHeaderPlanOrderReferenceInvalid", 400, cancellationToken);
            }

            var consumption = new PrOrderConsumption
            {
                BranchCode = branchCode,
                OrderId = orderId,
                StockCode = consumptionDto.StockCode.Trim(),
                YapKod = NullIfWhiteSpace(consumptionDto.YapKod),
                PlannedQuantity = consumptionDto.PlannedQuantity,
                Unit = NullIfWhiteSpace(consumptionDto.Unit),
                TrackingMode = string.IsNullOrWhiteSpace(consumptionDto.TrackingMode) ? "None" : consumptionDto.TrackingMode.Trim(),
                SerialEntryMode = string.IsNullOrWhiteSpace(consumptionDto.SerialEntryMode) ? "Optional" : consumptionDto.SerialEntryMode.Trim(),
                SourceWarehouseCode = NullIfWhiteSpace(consumptionDto.SourceWarehouseCode),
                SourceCellCode = NullIfWhiteSpace(consumptionDto.SourceCellCode),
                IsBackflush = consumptionDto.IsBackflush,
                IsMandatory = consumptionDto.IsMandatory,
                Status = "Draft",
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            };
            await _entityReferenceResolver.ResolveAsync(consumption, cancellationToken);
            consumptions.Add(consumption);
        }

        if (consumptions.Count > 0)
        {
            await _orderConsumptions.AddRangeAsync(consumptions, cancellationToken);
        }

        var dependencies = new List<PrOrderDependency>();
        foreach (var dependencyDto in request.Dependencies)
        {
            if (!orderIdByLocalId.TryGetValue(dependencyDto.PredecessorOrderLocalId, out var predecessorOrderId)
                || !orderIdByLocalId.TryGetValue(dependencyDto.SuccessorOrderLocalId, out var successorOrderId)
                || predecessorOrderId == successorOrderId)
            {
                return await RollbackPlanErrorAsync("PrHeaderPlanDependencyInvalid", 400, cancellationToken);
            }

            dependencies.Add(new PrOrderDependency
            {
                BranchCode = branchCode,
                HeaderId = header.Id,
                PredecessorOrderId = predecessorOrderId,
                SuccessorOrderId = successorOrderId,
                DependencyType = string.IsNullOrWhiteSpace(dependencyDto.DependencyType) ? "FinishToStart" : dependencyDto.DependencyType.Trim(),
                IsRequired = true,
                LagMinutes = dependencyDto.LagMinutes,
                RequiredTransferCompleted = dependencyDto.RequiredTransferCompleted,
                RequiredOutputAvailable = dependencyDto.RequiredOutputAvailable,
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            });
        }

        if (dependencies.Count > 0)
        {
            await _orderDependencies.AddRangeAsync(dependencies, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrHeaderDto>.SuccessResult(_mapper.Map<PrHeaderDto>(header), _localizationService.GetLocalizedString("PrHeaderPlanCreatedSuccessfully"));
    }

    private async Task<ApiResponse<PrHeaderDto>> RollbackPlanErrorAsync(string messageKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(messageKey);
        return ApiResponse<PrHeaderDto>.ErrorResult(message, message, statusCode);
    }

    private static string? NullIfWhiteSpace(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static byte? ClampPriorityLevel(int priority)
    {
        if (priority < byte.MinValue)
        {
            return byte.MinValue;
        }

        if (priority > byte.MaxValue)
        {
            return byte.MaxValue;
        }

        return (byte)priority;
    }

    private async Task<ApiResponse<T>> RollbackWithErrorAsync<T>(string messageKey, string exceptionKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(messageKey);
        var exception = _localizationService.GetLocalizedString(exceptionKey);
        return ApiResponse<T>.ErrorResult(message, exception, statusCode);
    }
}

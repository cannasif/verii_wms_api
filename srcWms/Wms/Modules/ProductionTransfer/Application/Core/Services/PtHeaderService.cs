using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Communications.Enums;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Domain.Entities.Production;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtHeaderService : IPtHeaderService
{
    private readonly IRepository<PtHeader> _headers;
    private readonly IRepository<PtLine> _lines;
    private readonly IRepository<PtImportLine> _importLines;
    private readonly IRepository<PtLineSerial> _lineSerials;
    private readonly IRepository<PtRoute> _routes;
    private readonly IRepository<PtTerminalLine> _terminalLines;
    private readonly IRepository<PrHeader> _productionHeaders;
    private readonly IRepository<PrOrder> _productionOrders;
    private readonly IRepository<PrOrderOutput> _productionOrderOutputs;
    private readonly IRepository<PrOrderConsumption> _productionOrderConsumptions;
    private readonly IRepository<PtParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly INotificationService _notificationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public PtHeaderService(
        IRepository<PtHeader> headers,
        IRepository<PtLine> lines,
        IRepository<PtImportLine> importLines,
        IRepository<PtLineSerial> lineSerials,
        IRepository<PtRoute> routes,
        IRepository<PtTerminalLine> terminalLines,
        IRepository<PrHeader> productionHeaders,
        IRepository<PrOrder> productionOrders,
        IRepository<PrOrderOutput> productionOrderOutputs,
        IRepository<PrOrderConsumption> productionOrderConsumptions,
        IRepository<PtParameter> parameters,
        IRepository<Notification> notifications,
        IRepository<PHeader> packageHeaders,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor,
        INotificationService notificationService,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _headers = headers;
        _lines = lines;
        _importLines = importLines;
        _lineSerials = lineSerials;
        _routes = routes;
        _terminalLines = terminalLines;
        _productionHeaders = productionHeaders;
        _productionOrders = productionOrders;
        _productionOrderOutputs = productionOrderOutputs;
        _productionOrderConsumptions = productionOrderConsumptions;
        _parameters = parameters;
        _notifications = notifications;
        _packageHeaders = packageHeaders;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
        _notificationService = notificationService;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var entities = await _headers.Query().Where(x => x.BranchCode == branchCode && !x.IsDeleted).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtHeaderDto>>(entities);
        await PopulateDeleteEligibilityAsync(entities, dtos, cancellationToken);
        return ApiResponse<IEnumerable<PtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
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
        var dtos = _mapper.Map<List<PtHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);
        return ApiResponse<PagedResponse<PtHeaderDto>>.SuccessResult(new PagedResponse<PtHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<PtHeaderDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PtHeaderDto>(entity);
        var deleteState = await EvaluateDeleteStateAsync(entity, cancellationToken);
        dto.CanDelete = deleteState.CanDelete;
        dto.DeleteBlockedReason = deleteState.Reason;
        return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtHeaderDto>> CreateAsync(CreatePtHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtHeader>(createDto) ?? new PtHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.BranchCode = string.IsNullOrWhiteSpace(createDto.BranchCode) ? (_currentUserAccessor.BranchCode ?? "0") : createDto.BranchCode;
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PtHeaderDto>(entity);
        return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtHeaderDto>> UpdateAsync(long id, UpdatePtHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<PtHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PtHeaderDto>(entity);
        return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (entity.IsCompleted)
        {
            var msg = _localizationService.GetLocalizedString("PtHeaderDeleteOnlyOpen");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        if (hasImportLines)
        {
            var msg = _localizationService.GetLocalizedString("PtHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("PtHeaderNotFound");
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
                .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.PT)
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
                    Title = _localizationService.GetLocalizedString("PtDoneNotificationTitle", orderNumber),
                    Message = _localizationService.GetLocalizedString("PtDoneNotificationMessage", orderNumber),
                    TitleKey = "PtDoneNotificationTitle",
                    MessageKey = "PtDoneNotificationMessage",
                    Channel = NotificationChannel.Web,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = entity.CreatedBy.Value,
                    RelatedEntityType = NotificationEntityType.PTDone,
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

            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtHeaderCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAssignedProductionTransferOrdersAsync(long userId, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var entities = await _headers.Query()
            .Where(h => !h.IsCompleted && !h.IsDeleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PtHeaderDto>>(entities);
        await PopulateDeleteEligibilityAsync(entities, dtos, cancellationToken);
        return ApiResponse<IEnumerable<PtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetAssignedProductionTransferOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
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
        var dtos = _mapper.Map<List<PtHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);
        return ApiResponse<PagedResponse<PtHeaderDto>>.SuccessResult(new PagedResponse<PtHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PtHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtAssignedProductionTransferOrderLinesDto>> GetAssignedProductionTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var lines = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var lineIds = lines.Select(x => x.Id).ToList();

        IEnumerable<PtLineSerial> lineSerials = Array.Empty<PtLineSerial>();
        if (lineIds.Count > 0)
        {
            lineSerials = await _lineSerials.Query().Where(x => lineIds.Contains(x.LineId) && !x.IsDeleted).ToListAsync(cancellationToken);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId && !x.IsDeleted).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();

        IEnumerable<PtRoute> routes = Array.Empty<PtRoute>();
        if (importLineIds.Count > 0)
        {
            routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId) && !x.IsDeleted).ToListAsync(cancellationToken);
        }

        var lineDtos = _mapper.Map<List<PtLineDto>>(lines);
        if (lineDtos.Count > 0)
        {
        }
        var importLineDtos = _mapper.Map<List<PtImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
        }

        var dto = new PtAssignedProductionTransferOrderLinesDto
        {
            Lines = lineDtos,
            LineSerials = _mapper.Map<List<PtLineSerialDto>>(lineSerials),
            ImportLines = importLineDtos,
            Routes = _mapper.Map<List<PtRouteDto>>(routes)
        };

        return ApiResponse<PtAssignedProductionTransferOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderAssignedOrderLinesRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<List<ProductionTransferSuggestedLineDto>>> GetProductionTransferSuggestionsAsync(ProductionTransferSuggestionRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.TransferPurpose))
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionTransferSuggestionRequestInvalid");
            return ApiResponse<List<ProductionTransferSuggestedLineDto>>.ErrorResult(message, message, 400);
        }

        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        IQueryable<PrOrder> orderQuery = _productionOrders.Query().Where(x => x.BranchCode == branchCode && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.ProductionDocumentNo))
        {
            var header = await _productionHeaders.Query()
                .Where(x => x.BranchCode == branchCode && x.DocumentNo == request.ProductionDocumentNo!.Trim() && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
            if (header == null)
            {
                var message = _localizationService.GetLocalizedString("PtHeaderProductionHeaderNotFound");
                return ApiResponse<List<ProductionTransferSuggestedLineDto>>.ErrorResult(message, message, 404);
            }

            orderQuery = orderQuery.Where(x => x.HeaderId == header.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.ProductionOrderNo))
        {
            orderQuery = orderQuery.Where(x => x.OrderNo == request.ProductionOrderNo!.Trim());
        }

        var orders = await orderQuery.OrderBy(x => x.SequenceNo ?? int.MaxValue).ThenBy(x => x.Id).ToListAsync(cancellationToken);
        if (orders.Count == 0)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionOrderNotFound");
            return ApiResponse<List<ProductionTransferSuggestedLineDto>>.ErrorResult(message, message, 404);
        }

        var orderIds = orders.Select(x => x.Id).ToList();
        var suggestions = new List<ProductionTransferSuggestedLineDto>();
        var purpose = request.TransferPurpose.Trim();

        if (string.Equals(purpose, "MaterialSupply", StringComparison.OrdinalIgnoreCase))
        {
            var consumptions = await _productionOrderConsumptions.Query()
                .Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted)
                .OrderBy(x => x.OrderId)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            suggestions.AddRange(consumptions.Select(x => new ProductionTransferSuggestedLineDto
            {
                StockCode = x.StockCode,
                YapKod = x.YapKod,
                Quantity = Math.Max(0m, x.PlannedQuantity - (x.ConsumedQuantity ?? 0m)),
                LineRole = "ConsumptionSupply",
                ProductionOrderNo = orders.First(o => o.Id == x.OrderId).OrderNo,
                SourceWarehouseCode = x.SourceWarehouseCode,
                SourceCellCode = x.SourceCellCode,
                TargetWarehouseCode = orders.First(o => o.Id == x.OrderId).SourceWarehouseCode
            }).Where(x => x.Quantity > 0));
        }
        else
        {
            var outputs = await _productionOrderOutputs.Query()
                .Where(x => orderIds.Contains(x.OrderId) && !x.IsDeleted)
                .OrderBy(x => x.OrderId)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            var role = string.Equals(purpose, "SemiFinishedMove", StringComparison.OrdinalIgnoreCase) ? "SemiFinishedMove" : "OutputMove";
            suggestions.AddRange(outputs.Select(x => new ProductionTransferSuggestedLineDto
            {
                StockCode = x.StockCode,
                YapKod = x.YapKod,
                Quantity = Math.Max(0m, (x.ProducedQuantity ?? x.PlannedQuantity)),
                LineRole = role,
                ProductionOrderNo = orders.First(o => o.Id == x.OrderId).OrderNo,
                SourceWarehouseCode = orders.First(o => o.Id == x.OrderId).TargetWarehouseCode,
                TargetWarehouseCode = x.TargetWarehouseCode,
                TargetCellCode = x.TargetCellCode
            }).Where(x => x.Quantity > 0));
        }

        return ApiResponse<List<ProductionTransferSuggestedLineDto>>.SuccessResult(suggestions, _localizationService.GetLocalizedString("PtHeaderProductionTransferSuggestionsRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ProductionTransferDetailDto>> GetProductionTransferDetailAsync(long id, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var header = await _headers.Query()
            .Where(x => x.Id == id && x.BranchCode == branchCode && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (header == null)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<ProductionTransferDetailDto>.ErrorResult(message, message, 404);
        }

        var lines = await _lines.Query()
            .Where(x => x.HeaderId == header.Id && !x.IsDeleted)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var productionOrderIds = lines.Where(x => x.ProductionOrderId.HasValue).Select(x => x.ProductionOrderId!.Value).Distinct().ToList();
        var orderNoById = productionOrderIds.Count == 0
            ? new Dictionary<long, string>()
            : await _productionOrders.Query()
                .Where(x => productionOrderIds.Contains(x.Id) && !x.IsDeleted)
                .ToDictionaryAsync(x => x.Id, x => x.OrderNo, cancellationToken);

        string? productionDocumentNo = null;
        if (header.ProductionHeaderId.HasValue)
        {
            productionDocumentNo = await _productionHeaders.Query()
                .Where(x => x.Id == header.ProductionHeaderId.Value && !x.IsDeleted)
                .Select(x => x.DocumentNo)
                .FirstOrDefaultAsync(cancellationToken);
        }

        string? productionOrderNo = null;
        if (header.ProductionOrderId.HasValue)
        {
            orderNoById.TryGetValue(header.ProductionOrderId.Value, out productionOrderNo);
        }

        var detail = new ProductionTransferDetailDto
        {
            Id = header.Id,
            DocumentNo = header.DocumentNo ?? string.Empty,
            DocumentDate = header.DocumentDate,
            TransferPurpose = string.IsNullOrWhiteSpace(header.TransferPurpose) ? "MaterialSupply" : header.TransferPurpose,
            ProductionDocumentNo = productionDocumentNo,
            ProductionOrderNo = productionOrderNo,
            SourceWarehouseCode = header.SourceWarehouse,
            TargetWarehouseCode = header.TargetWarehouse,
            Description = header.Description1,
            IsCompleted = header.IsCompleted,
            Lines = lines.Select(line => new ProductionTransferDetailLineDto
            {
                Id = line.Id,
                StockCode = line.StockCode,
                YapKod = line.YapKod,
                Quantity = line.Quantity,
                LineRole = string.IsNullOrWhiteSpace(line.LineRole) ? "ConsumptionSupply" : line.LineRole,
                ProductionOrderNo = line.ProductionOrderId.HasValue && orderNoById.TryGetValue(line.ProductionOrderId.Value, out var orderNo) ? orderNo : null
            }).ToList()
        };

        var deleteState = await EvaluateDeleteStateAsync(header, cancellationToken);
        detail.CanDelete = deleteState.CanDelete;
        detail.DeleteBlockedReason = deleteState.Reason;

        return ApiResponse<ProductionTransferDetailDto>.SuccessResult(detail, _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtHeaderDto>> CreateProductionTransferAsync(CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.DocumentNo) || request.Lines.Count == 0)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionTransferRequestInvalid");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 400);
        }

        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var currentUserId = _currentUserAccessor.UserId;
        var documentNo = request.DocumentNo.Trim();

        var duplicateDocument = await _headers.Query().AnyAsync(x => x.BranchCode == branchCode && x.DocumentNo == documentNo && !x.IsDeleted, cancellationToken);
        if (duplicateDocument)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionTransferDocumentNoAlreadyExists");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 409);
        }

        PrHeader? productionHeader = null;
        if (!string.IsNullOrWhiteSpace(request.ProductionDocumentNo))
        {
            productionHeader = await _productionHeaders.Query()
                .Where(x => x.BranchCode == branchCode && x.DocumentNo == request.ProductionDocumentNo!.Trim() && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (productionHeader == null)
            {
                var message = _localizationService.GetLocalizedString("PtHeaderProductionHeaderNotFound");
                return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 404);
            }
        }

        PrOrder? productionOrder = null;
        if (!string.IsNullOrWhiteSpace(request.ProductionOrderNo))
        {
            var productionOrderQuery = _productionOrders.Query().Where(x => x.BranchCode == branchCode && x.OrderNo == request.ProductionOrderNo!.Trim() && !x.IsDeleted);
            if (productionHeader != null)
            {
                productionOrderQuery = productionOrderQuery.Where(x => x.HeaderId == productionHeader.Id);
            }

            productionOrder = await productionOrderQuery.FirstOrDefaultAsync(cancellationToken);
            if (productionOrder == null)
            {
                var message = _localizationService.GetLocalizedString("PtHeaderProductionOrderNotFound");
                return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 404);
            }
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = new PtHeader
            {
                BranchCode = branchCode,
                DocumentNo = documentNo,
                DocumentDate = request.DocumentDate ?? DateTimeProvider.Now,
                DocumentType = "PT_PRODUCTION",
                Description1 = NullIfWhiteSpace(request.Description),
                SourceWarehouse = NullIfWhiteSpace(request.SourceWarehouseCode),
                TargetWarehouse = NullIfWhiteSpace(request.TargetWarehouseCode),
                ProductionHeaderId = productionHeader?.Id,
                ProductionOrderId = productionOrder?.Id,
                TransferPurpose = string.IsNullOrWhiteSpace(request.TransferPurpose) ? "MaterialSupply" : request.TransferPurpose.Trim(),
                TriggeredByProduction = productionHeader != null || productionOrder != null,
                AutoGenerated = false,
                RequiredForOrderStart = false,
                RequiredForOrderCompletion = false,
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = currentUserId
            };

            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lines = new List<PtLine>(request.Lines.Count);
            foreach (var lineDto in request.Lines)
            {
                if (string.IsNullOrWhiteSpace(lineDto.StockCode) || lineDto.Quantity <= 0)
                {
                    return await RollbackProductionTransferErrorAsync("PtHeaderProductionTransferLineInvalid", 400, cancellationToken);
                }

                PrOrder? lineOrder = productionOrder;
                if (!string.IsNullOrWhiteSpace(lineDto.ProductionOrderNo))
                {
                    var scopedOrderQuery = _productionOrders.Query()
                        .Where(x => x.BranchCode == branchCode && x.OrderNo == lineDto.ProductionOrderNo.Trim() && !x.IsDeleted);
                    if (productionHeader != null)
                    {
                        scopedOrderQuery = scopedOrderQuery.Where(x => x.HeaderId == productionHeader.Id);
                    }

                    lineOrder = await scopedOrderQuery.FirstOrDefaultAsync(cancellationToken);
                    if (lineOrder == null)
                    {
                        return await RollbackProductionTransferErrorAsync("PtHeaderProductionOrderNotFound", 404, cancellationToken);
                    }
                }

                var line = new PtLine
                {
                    BranchCode = branchCode,
                    HeaderId = header.Id,
                    StockCode = lineDto.StockCode.Trim(),
                    YapKod = NullIfWhiteSpace(lineDto.YapKod),
                    Quantity = lineDto.Quantity,
                    Description = lineDto.LineRole,
                    ProductionOrderId = lineOrder?.Id,
                    LineRole = string.IsNullOrWhiteSpace(lineDto.LineRole) ? "ConsumptionSupply" : lineDto.LineRole.Trim(),
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                };

                await _entityReferenceResolver.ResolveAsync(line, cancellationToken);

                if (lineOrder != null)
                {
                    if (string.Equals(line.LineRole, "ConsumptionSupply", StringComparison.OrdinalIgnoreCase))
                    {
                        var consumption = await _productionOrderConsumptions.Query()
                            .Where(x => x.OrderId == lineOrder.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                            .OrderBy(x => x.Id)
                            .FirstOrDefaultAsync(cancellationToken);
                        if (consumption != null)
                        {
                            line.ProductionOrderConsumptionId = consumption.Id;
                            line.RequiredQuantityFromProduction = consumption.PlannedQuantity;
                        }
                    }
                    else
                    {
                        var output = await _productionOrderOutputs.Query()
                            .Where(x => x.OrderId == lineOrder.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                            .OrderBy(x => x.Id)
                            .FirstOrDefaultAsync(cancellationToken);
                        if (output != null)
                        {
                            line.ProductionOrderOutputId = output.Id;
                            line.RequiredQuantityFromProduction = output.PlannedQuantity;
                        }
                    }
                }

                lines.Add(line);
            }

            await _lines.AddRangeAsync(lines, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var dto = _mapper.Map<PtHeaderDto>(header);
            return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderProductionTransferCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PtHeaderDto>> UpdateProductionTransferAsync(long id, CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.DocumentNo) || request.Lines.Count == 0)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionTransferRequestInvalid");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 400);
        }

        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var currentUserId = _currentUserAccessor.UserId;
        var header = await _headers.Query(tracking: true)
            .Where(x => x.Id == id && x.BranchCode == branchCode && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (header == null)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 404);
        }

        if (header.IsCompleted)
        {
            const string completedMessage = "Tamamlanmis uretim transfer emirleri duzenlenemez.";
            return ApiResponse<PtHeaderDto>.ErrorResult(completedMessage, completedMessage, 400);
        }

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        if (hasImportLines)
        {
            const string processedMessage = "Islem gormus uretim transfer emirleri duzenlenemez.";
            return ApiResponse<PtHeaderDto>.ErrorResult(processedMessage, processedMessage, 400);
        }

        var duplicateDocument = await _headers.Query().AnyAsync(
            x => x.BranchCode == branchCode && x.DocumentNo == request.DocumentNo.Trim() && x.Id != id && !x.IsDeleted,
            cancellationToken);
        if (duplicateDocument)
        {
            var message = _localizationService.GetLocalizedString("PtHeaderProductionTransferDocumentNoAlreadyExists");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 409);
        }

        PrHeader? productionHeader = null;
        if (!string.IsNullOrWhiteSpace(request.ProductionDocumentNo))
        {
            productionHeader = await _productionHeaders.Query()
                .Where(x => x.BranchCode == branchCode && x.DocumentNo == request.ProductionDocumentNo!.Trim() && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (productionHeader == null)
            {
                var message = _localizationService.GetLocalizedString("PtHeaderProductionHeaderNotFound");
                return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 404);
            }
        }

        PrOrder? productionOrder = null;
        if (!string.IsNullOrWhiteSpace(request.ProductionOrderNo))
        {
            var productionOrderQuery = _productionOrders.Query().Where(x => x.BranchCode == branchCode && x.OrderNo == request.ProductionOrderNo!.Trim() && !x.IsDeleted);
            if (productionHeader != null)
            {
                productionOrderQuery = productionOrderQuery.Where(x => x.HeaderId == productionHeader.Id);
            }

            productionOrder = await productionOrderQuery.FirstOrDefaultAsync(cancellationToken);
            if (productionOrder == null)
            {
                var message = _localizationService.GetLocalizedString("PtHeaderProductionOrderNotFound");
                return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 404);
            }
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            header.DocumentNo = request.DocumentNo.Trim();
            header.DocumentDate = request.DocumentDate ?? header.DocumentDate ?? DateTimeProvider.Now;
            header.DocumentType = "PT_PRODUCTION";
            header.Description1 = NullIfWhiteSpace(request.Description);
            header.SourceWarehouse = NullIfWhiteSpace(request.SourceWarehouseCode);
            header.TargetWarehouse = NullIfWhiteSpace(request.TargetWarehouseCode);
            header.ProductionHeaderId = productionHeader?.Id;
            header.ProductionOrderId = productionOrder?.Id;
            header.TransferPurpose = string.IsNullOrWhiteSpace(request.TransferPurpose) ? "MaterialSupply" : request.TransferPurpose.Trim();
            header.TriggeredByProduction = productionHeader != null || productionOrder != null;
            header.AutoGenerated = false;
            header.UpdatedDate = DateTimeProvider.Now;
            header.UpdatedBy = currentUserId;

            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            _headers.Update(header);

            var existingLines = await _lines.Query(tracking: true)
                .Where(x => x.HeaderId == header.Id && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var existingLine in existingLines)
            {
                existingLine.IsDeleted = true;
                existingLine.DeletedDate = DateTimeProvider.Now;
                existingLine.DeletedBy = currentUserId;
                _lines.Update(existingLine);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lines = new List<PtLine>(request.Lines.Count);
            foreach (var lineDto in request.Lines)
            {
                if (string.IsNullOrWhiteSpace(lineDto.StockCode) || lineDto.Quantity <= 0)
                {
                    return await RollbackProductionTransferErrorAsync("PtHeaderProductionTransferLineInvalid", 400, cancellationToken);
                }

                PrOrder? lineOrder = productionOrder;
                if (!string.IsNullOrWhiteSpace(lineDto.ProductionOrderNo))
                {
                    var scopedOrderQuery = _productionOrders.Query()
                        .Where(x => x.BranchCode == branchCode && x.OrderNo == lineDto.ProductionOrderNo.Trim() && !x.IsDeleted);
                    if (productionHeader != null)
                    {
                        scopedOrderQuery = scopedOrderQuery.Where(x => x.HeaderId == productionHeader.Id);
                    }

                    lineOrder = await scopedOrderQuery.FirstOrDefaultAsync(cancellationToken);
                    if (lineOrder == null)
                    {
                        return await RollbackProductionTransferErrorAsync("PtHeaderProductionOrderNotFound", 404, cancellationToken);
                    }
                }

                var line = new PtLine
                {
                    BranchCode = branchCode,
                    HeaderId = header.Id,
                    StockCode = lineDto.StockCode.Trim(),
                    YapKod = NullIfWhiteSpace(lineDto.YapKod),
                    Quantity = lineDto.Quantity,
                    Description = lineDto.LineRole,
                    ProductionOrderId = lineOrder?.Id,
                    LineRole = string.IsNullOrWhiteSpace(lineDto.LineRole) ? "ConsumptionSupply" : lineDto.LineRole.Trim(),
                    CreatedDate = DateTimeProvider.Now,
                    CreatedBy = currentUserId
                };

                await _entityReferenceResolver.ResolveAsync(line, cancellationToken);

                if (lineOrder != null)
                {
                    if (string.Equals(line.LineRole, "ConsumptionSupply", StringComparison.OrdinalIgnoreCase))
                    {
                        var consumption = await _productionOrderConsumptions.Query()
                            .Where(x => x.OrderId == lineOrder.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                            .OrderBy(x => x.Id)
                            .FirstOrDefaultAsync(cancellationToken);
                        if (consumption != null)
                        {
                            line.ProductionOrderConsumptionId = consumption.Id;
                            line.RequiredQuantityFromProduction = consumption.PlannedQuantity;
                        }
                    }
                    else
                    {
                        var output = await _productionOrderOutputs.Query()
                            .Where(x => x.OrderId == lineOrder.Id && x.StockCode == line.StockCode && (x.YapKod ?? "") == (line.YapKod ?? "") && !x.IsDeleted)
                            .OrderBy(x => x.Id)
                            .FirstOrDefaultAsync(cancellationToken);
                        if (output != null)
                        {
                            line.ProductionOrderOutputId = output.Id;
                            line.RequiredQuantityFromProduction = output.PlannedQuantity;
                        }
                    }
                }

                lines.Add(line);
            }

            await _lines.AddRangeAsync(lines, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var dto = _mapper.Map<PtHeaderDto>(header);
            return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderUpdatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PtHeaderDto>> GenerateProductionTransferOrderAsync(GenerateProductionTransferOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<PtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<PtHeader>(request.Header) ?? new PtHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();

            if (request.Lines?.Count > 0)
            {
                var lines = new List<PtLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<PtLine>(lineDto) ?? new PtLine();
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
                var serials = new List<PtLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<PtLineSerial>(serialDto) ?? new PtLineSerial();
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
                        var terminalLine = _mapper.Map<PtTerminalLine>(x) ?? new PtTerminalLine();
                        terminalLine.HeaderId = header.Id;
                        return terminalLine;
                    })
                    .ToList();

                await _terminalLines.AddRangeAsync(terminalLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                    terminalLines,
                    header.Id.ToString(),
                    NotificationEntityType.PTHeader,
                    "PT_HEADER",
                    "PtHeaderNotificationTitle",
                    "PtHeaderNotificationMessage",
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

            var dto = _mapper.Map<PtHeaderDto>(header);
            return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PtHeaderDto>> BulkPtGenerateAsync(BulkPtGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            return ApiResponse<PtHeaderDto>.ErrorResult(
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
            var header = _mapper.Map<PtHeader>(request.Header) ?? new PtHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (header.Id <= 0)
            {
                return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderCreationError", "HeaderInsertFailed", 500, cancellationToken);
            }

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();
            if (request.Lines?.Count > 0)
            {
                var lines = new List<PtLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<PtLine>(lineDto) ?? new PtLine();
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
                var serials = new List<PtLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "PtHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<PtLineSerial>(serialDto) ?? new PtLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.ImportLines?.Count > 0)
            {
                var importLines = new List<PtImportLine>(request.ImportLines.Count);
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

                    var importLine = new PtImportLine
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
                var routes = new List<PtRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    long importLineId;
                    if (routeDto.ImportLineGroupGuid.HasValue)
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineGroupGuid.Value.ToString(), out importLineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "ImportLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey!, out importLineId))
                        {
                            return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "ImportLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<PtHeaderDto>("PtHeaderInvalidCorrelationKey", "ImportLineReferenceMissing", 400, cancellationToken);
                    }

                    var route = new PtRoute
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
            var dto = _mapper.Map<PtHeaderDto>(header);
            return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderBulkPtGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => !x.IsDeleted && x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtHeaderDto>>(items);
        await PopulateDeleteEligibilityAsync(items, dtos, cancellationToken);

        var result = new PagedResponse<PtHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<PtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
    }

    private async Task PopulateDeleteEligibilityAsync(IReadOnlyCollection<PtHeader> entities, IReadOnlyCollection<PtHeaderDto> dtos, CancellationToken cancellationToken)
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

        var importLineSet = importLineHeaderIds.ToHashSet();
        var dtoById = dtos.ToDictionary(x => x.Id);

        foreach (var entity in entities)
        {
            if (!dtoById.TryGetValue(entity.Id, out var dto))
            {
                continue;
            }

            var state = EvaluateDeleteState(entity, importLineSet.Contains(entity.Id));
            dto.CanDelete = state.CanDelete;
            dto.DeleteBlockedReason = state.Reason;
        }
    }

    private async Task<(bool CanDelete, string? Reason)> EvaluateDeleteStateAsync(PtHeader entity, CancellationToken cancellationToken)
    {
        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == entity.Id && !x.IsDeleted, cancellationToken);
        return EvaluateDeleteState(entity, hasImportLines);
    }

    private (bool CanDelete, string? Reason) EvaluateDeleteState(PtHeader entity, bool hasImportLines)
    {
        if (entity.IsDeleted)
        {
            return (false, _localizationService.GetLocalizedString("PtHeaderNotFound"));
        }

        if (entity.IsCompleted)
        {
            return (false, _localizationService.GetLocalizedString("PtHeaderDeleteOnlyOpen"));
        }

        if (hasImportLines)
        {
            return (false, _localizationService.GetLocalizedString("PtHeaderImportLinesExist"));
        }

        return (true, null);
    }

    public async Task<ApiResponse<PtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var notFound = _localizationService.GetLocalizedString("PtHeaderNotFound");
            return ApiResponse<PtHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
        {
            var message = _localizationService.GetLocalizedString("PtHeaderApprovalUpdateError");
            return ApiResponse<PtHeaderDto>.ErrorResult(message, message, 400);
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

        var dto = _mapper.Map<PtHeaderDto>(entity);
        return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderApprovalUpdatedSuccessfully"));
    }

    private async Task<ApiResponse<bool>?> ValidateLineSerialVsRouteQuantitiesAsync(long headerId, PtParameter? parameter, CancellationToken cancellationToken)
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
                    var msg = _localizationService.GetLocalizedString("PtHeaderAllOrderItemsMustBeCollected", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty);
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
                LineSerialRouteValidationMismatch.ExactMatchRequired => "PtHeaderQuantityExactMatchRequired",
                LineSerialRouteValidationMismatch.CannotBeGreater => "PtHeaderQuantityCannotBeGreater",
                LineSerialRouteValidationMismatch.CannotBeLess => "PtHeaderQuantityCannotBeLess",
                _ => "PtHeaderQuantityExactMatchRequired"
            };

            var validationMessage = _localizationService.GetLocalizedString(messageKey, line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, validationResult.ExpectedQuantity, validationResult.ActualQuantity);
            return ApiResponse<bool>.ErrorResult(validationMessage, validationMessage, 400);
        }

        return null;
    }

    private async Task<ApiResponse<PtHeaderDto>> RollbackProductionTransferErrorAsync(string messageKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(messageKey);
        return ApiResponse<PtHeaderDto>.ErrorResult(message, message, statusCode);
    }

    private static string? NullIfWhiteSpace(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private async Task<ApiResponse<T>> RollbackWithErrorAsync<T>(string messageKey, string exceptionKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(messageKey);
        var exception = _localizationService.GetLocalizedString(exceptionKey);
        return ApiResponse<T>.ErrorResult(message, exception, statusCode);
    }
}

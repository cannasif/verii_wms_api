using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Application.ServiceAllocation.Services;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Communications.Enums;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Services;

public sealed class WtHeaderService : IWtHeaderService
{
    private readonly IRepository<WtHeader> _headers;
    private readonly IRepository<WtLine> _lines;
    private readonly IRepository<WtImportLine> _importLines;
    private readonly IRepository<WtLineSerial> _lineSerials;
    private readonly IRepository<WtRoute> _routes;
    private readonly IRepository<WtTerminalLine> _terminalLines;
    private readonly IRepository<WtParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly INotificationService _notificationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IOperationAllocationOrchestrator _operationAllocationOrchestrator;

    public WtHeaderService(
        IRepository<WtHeader> headers,
        IRepository<WtLine> lines,
        IRepository<WtImportLine> importLines,
        IRepository<WtLineSerial> lineSerials,
        IRepository<WtRoute> routes,
        IRepository<WtTerminalLine> terminalLines,
        IRepository<WtParameter> parameters,
        IRepository<Notification> notifications,
        IRepository<PHeader> packageHeaders,
        IUnitOfWork unitOfWtrk,
        IMapper mapper,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor,
        INotificationService notificationService,
        IEntityReferenceResolver entityReferenceResolver,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IOperationAllocationOrchestrator operationAllocationOrchestrator)
    {
        _headers = headers;
        _lines = lines;
        _importLines = importLines;
        _lineSerials = lineSerials;
        _routes = routes;
        _terminalLines = terminalLines;
        _parameters = parameters;
        _notifications = notifications;
        _packageHeaders = packageHeaders;
        _unitOfWork = unitOfWtrk;
        _mapper = mapper;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
        _notificationService = notificationService;
        _entityReferenceResolver = entityReferenceResolver;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _operationAllocationOrchestrator = operationAllocationOrchestrator;
    }

    public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var entities = await _headers.Query().Where(x => x.BranchCode == branchCode).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtHeaderDto>>(entities);
        return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var branchCode = _currentUserAccessor.BranchCode ?? "0";

        var query = _headers.Query()
            .Where(x => x.BranchCode == branchCode)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtHeaderDto>>(items);
        return ApiResponse<PagedResponse<WtHeaderDto>>.SuccessResult(new PagedResponse<WtHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtHeaderNotFound");
            return ApiResponse<WtHeaderDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WtHeaderDto>(entity);
        await _documentReferenceReadEnricher.EnrichHeadersAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtHeaderDto>> CreateAsync(CreateWtHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WtHeader>(createDto) ?? new WtHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.BranchCode = string.IsNullOrWhiteSpace(createDto.BranchCode) ? (_currentUserAccessor.BranchCode ?? "0") : createDto.BranchCode;
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<WtHeaderDto>(entity);
        return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WtHeaderDto>> UpdateAsync(long id, UpdateWtHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtHeaderNotFound");
            return ApiResponse<WtHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<WtHeaderDto>(entity);
        return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        if (hasImportLines)
        {
            var msg = _localizationService.GetLocalizedString("WtHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
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
                .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.WT)
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
                    Title = _localizationService.GetLocalizedString("WtDoneNotificationTitle", orderNumber),
                    Message = _localizationService.GetLocalizedString("WtDoneNotificationMessage", orderNumber),
                    TitleKey = "WtDoneNotificationTitle",
                    MessageKey = "WtDoneNotificationMessage",
                    Channel = NotificationChannel.Web,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = entity.CreatedBy.Value,
                    RelatedEntityType = NotificationEntityType.WTDone,
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

            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtHeaderCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<WtHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var entities = await _headers.Query()
            .Where(h => !h.IsCompleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<WtHeaderDto>>(entities);
        return ApiResponse<IEnumerable<WtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var query = _headers.Query()
            .Where(h => !h.IsCompleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtHeaderDto>>(items);
        return ApiResponse<PagedResponse<WtHeaderDto>>.SuccessResult(new PagedResponse<WtHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("WtHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var lines = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var lineIds = lines.Select(x => x.Id).ToList();

        IEnumerable<WtLineSerial> lineSerials = Array.Empty<WtLineSerial>();
        if (lineIds.Count > 0)
        {
            lineSerials = await _lineSerials.Query().Where(x => lineIds.Contains(x.LineId)).ToListAsync(cancellationToken);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();

        IEnumerable<WtRoute> routes = Array.Empty<WtRoute>();
        if (importLineIds.Count > 0)
        {
            routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);
        }

        var lineDtos = _mapper.Map<List<WtLineDto>>(lines);
        if (lineDtos.Count > 0)
        {
        }
        var importLineDtos = _mapper.Map<List<WtImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
        }

        var dto = new WtAssignedOrderLinesDto
        {
            Lines = lineDtos,
            LineSerials = _mapper.Map<List<WtLineSerialDto>>(lineSerials),
            ImportLines = importLineDtos,
            Routes = _mapper.Map<List<WtRouteDto>>(routes)
        };

        return ApiResponse<WtAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderAssignedOrderLinesRetrievedSuccessfully"));
    }

    public Task<ApiResponse<WtAssignedOrderLinesDto>> GetAssignedTransferOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        return GetAssignedOrderLinesAsync(headerId, cancellationToken);
    }

    public async Task<ApiResponse<WtHeaderDto>> GenerateWarehouseTransferOrderAsync(GenerateWarehouseTransferOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<WtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<WtHeader>(request.Header) ?? new WtHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();

            if (request.Lines?.Count > 0)
            {
                var lines = new List<WtLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<WtLine>(lineDto) ?? new WtLine();
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
                var serials = new List<WtLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<WtLineSerial>(serialDto) ?? new WtLineSerial();
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
                        var terminalLine = _mapper.Map<WtTerminalLine>(x) ?? new WtTerminalLine();
                        terminalLine.HeaderId = header.Id;
                        return terminalLine;
                    })
                    .ToList();

                await _terminalLines.AddRangeAsync(terminalLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                    terminalLines,
                    header.Id.ToString(),
                    NotificationEntityType.WTHeader,
                    "WT_HEADER",
                    "WtHeaderNotificationTitle",
                    "WtHeaderNotificationMessage",
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

            var dto = _mapper.Map<WtHeaderDto>(header);
            return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<WtHeaderDto>> BulkCreateWarehouseTransferAsync(BulkCreateWtRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            return ApiResponse<WtHeaderDto>.ErrorResult(
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
            var header = _mapper.Map<WtHeader>(request.Header) ?? new WtHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (header.Id <= 0)
            {
                return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderCreationError", "HeaderInsertFailed", 500, cancellationToken);
            }

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();
            if (request.Lines?.Count > 0)
            {
                var lines = new List<WtLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<WtLine>(lineDto) ?? new WtLine();
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
                var serials = new List<WtLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "WtHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<WtLineSerial>(serialDto) ?? new WtLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.ImportLines?.Count > 0)
            {
                var importLines = new List<WtImportLine>(request.ImportLines.Count);
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

                    var importLine = new WtImportLine
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
                var routes = new List<WtRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    long importLineId;
                    if (routeDto.ImportLineGroupGuid.HasValue)
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineGroupGuid.Value.ToString(), out importLineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "ImportLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey!, out importLineId))
                        {
                            return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "ImportLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<WtHeaderDto>("WtHeaderInvalidCorrelationKey", "ImportLineReferenceMissing", 400, cancellationToken);
                    }

                    var route = new WtRoute
                    {
                        ImportLineId = importLineId,
                        Quantity = routeDto.Quantity,
                        SerialNo = routeDto.SerialNo,
                        SerialNo2 = routeDto.SerialNo2,
                        SerialNo3 = routeDto.SerialNo3,
                        SerialNo4 = routeDto.SerialNo4,
                        ScannedBarcode = routeDto.ScannedBarcode ?? string.Empty,
                        SourceWarehouse = routeDto.SourceWarehouse.HasValue ? (int?)routeDto.SourceWarehouse.Value : null,
                        TargetWarehouse = routeDto.TargetWarehouse.HasValue ? (int?)routeDto.TargetWarehouse.Value : null,
                        SourceCellCode = routeDto.SourceCellCode,
                        TargetCellCode = routeDto.TargetCellCode
                    };
                    routes.Add(route);
                }

                await _routes.AddRangeAsync(routes, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            var dto = _mapper.Map<WtHeaderDto>(header);
            return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderBulkWtGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public Task<ApiResponse<WtHeaderDto>> BulkWtGenerateAsync(BulkCreateWtRequestDto request, CancellationToken cancellationToken = default)
    {
        return BulkCreateWarehouseTransferAsync(request, cancellationToken);
    }

    public async Task<ApiResponse<int>> ProcessWarehouseTransferAsync(ProcessWtRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            return ApiResponse<int>.ErrorResult(
                _localizationService.GetLocalizedString("InvalidModelState"),
                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                400);
        }

        if (request.Routes?.Any(x => x.Quantity <= 0) == true)
        {
            var message = _localizationService.GetLocalizedString("InvalidModelState");
            return ApiResponse<int>.ErrorResult(message, "Quantity must be greater than zero for transfer process routes.", 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var parameter = await _parameters.Query().FirstOrDefaultAsync(cancellationToken);
            var header = _mapper.Map<WtHeader>(request.Header) ?? new WtHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (header.Id <= 0)
            {
                return await RollbackWithErrorAsync<int>("WtHeaderCreationError", "HeaderInsertFailed", 500, cancellationToken);
            }

            var importLineGroupingKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

            if (request.Routes?.Count > 0)
            {
                var groupedImportLines = WtProcessGroupingHelper.BuildImportLineSeeds(request.Routes);

                var importLines = new List<WtImportLine>(groupedImportLines.Count);
                foreach (var grouped in groupedImportLines)
                {
                    var importLine = new WtImportLine
                    {
                        HeaderId = header.Id,
                        LineId = null,
                        StockCode = grouped.StockCode,
                        StockId = grouped.StockId,
                        YapKod = grouped.YapKod,
                        YapKodId = grouped.YapKodId
                    };
                    await _entityReferenceResolver.ResolveAsync(importLine, cancellationToken);
                    importLines.Add(importLine);
                }

                await _importLines.AddRangeAsync(importLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var groupedKeys = groupedImportLines.Select(x => x.GroupingKey).ToArray();
                for (var i = 0; i < groupedKeys.Length; i++)
                {
                    importLineGroupingKeyToId[groupedKeys[i]] = importLines[i].Id;
                }

                var routes = new List<WtRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    var groupingKey = WtProcessGroupingHelper.BuildGroupingKey(routeDto.StockId, routeDto.StockCode, routeDto.YapKodId, routeDto.YapKod);
                    if (!importLineGroupingKeyToId.TryGetValue(groupingKey, out var importLineId))
                    {
                        return await RollbackWithErrorAsync<int>("WtHeaderCreationError", "ImportLineReferenceMissing", 400, cancellationToken);
                    }

                    routes.Add(new WtRoute
                    {
                        ImportLineId = importLineId,
                        Quantity = routeDto.Quantity,
                        ScannedBarcode = routeDto.ScannedBarcode ?? string.Empty,
                        SerialNo = routeDto.SerialNo,
                        SerialNo2 = routeDto.SerialNo2,
                        SerialNo3 = routeDto.SerialNo3,
                        SerialNo4 = routeDto.SerialNo4,
                        SourceWarehouse = routeDto.SourceWarehouse.HasValue ? (int?)routeDto.SourceWarehouse.Value : null,
                        TargetWarehouse = routeDto.TargetWarehouse.HasValue ? (int?)routeDto.TargetWarehouse.Value : null,
                        SourceCellCode = routeDto.SourceCellCode,
                        TargetCellCode = routeDto.TargetCellCode
                    });
                }

                await _routes.AddRangeAsync(routes, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            await _operationAllocationOrchestrator.TriggerForDocumentAsync(Wms.Domain.Entities.ServiceAllocation.Enums.DocumentModule.WT, header.Id, cancellationToken);
            return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("WtHeaderBulkWtGenerateCompletedSuccessfully"));
        }
        catch (DbUpdateException ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            var exceptionMessage = ex.InnerException?.Message ?? ex.Message;
            return ApiResponse<int>.ErrorResult(
                _localizationService.GetLocalizedString("WtHeaderCreationError"),
                exceptionMessage,
                400);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PagedResponse<WtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtHeaderDto>>(items);

        await _documentReferenceReadEnricher.EnrichHeadersAsync(dtos, cancellationToken);

        var result = new PagedResponse<WtHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<WtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("WtHeaderNotFound");
            return ApiResponse<WtHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
        {
            var message = _localizationService.GetLocalizedString("WtHeaderApprovalUpdateError");
            return ApiResponse<WtHeaderDto>.ErrorResult(message, message, 400);
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

        var dto = _mapper.Map<WtHeaderDto>(entity);
        return ApiResponse<WtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtHeaderApprovalUpdatedSuccessfully"));
    }

    private async Task<ApiResponse<bool>?> ValidateLineSerialVsRouteQuantitiesAsync(long headerId, WtParameter? parameter, CancellationToken cancellationToken)
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
                    var msg = _localizationService.GetLocalizedString("WtHeaderAllOrderItemsMustBeCollected", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty);
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
                LineSerialRouteValidationMismatch.ExactMatchRequired => "WtHeaderQuantityExactMatchRequired",
                LineSerialRouteValidationMismatch.CannotBeGreater => "WtHeaderQuantityCannotBeGreater",
                LineSerialRouteValidationMismatch.CannotBeLess => "WtHeaderQuantityCannotBeLess",
                _ => "WtHeaderQuantityExactMatchRequired"
            };

            var validationMessage = _localizationService.GetLocalizedString(messageKey, line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, validationResult.ExpectedQuantity, validationResult.ActualQuantity);
            return ApiResponse<bool>.ErrorResult(validationMessage, validationMessage, 400);
        }

        return null;
    }

    private async Task<ApiResponse<T>> RollbackWithErrorAsync<T>(string messageKey, string exceptionKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(messageKey);
        var exception = _localizationService.GetLocalizedString(exceptionKey);
        return ApiResponse<T>.ErrorResult(message, exception, statusCode);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Communications.Enums;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoHeaderService : IWoHeaderService
{
    private readonly IRepository<WoHeader> _headers;
    private readonly IRepository<WoLine> _lines;
    private readonly IRepository<WoImportLine> _importLines;
    private readonly IRepository<WoLineSerial> _lineSerials;
    private readonly IRepository<WoRoute> _routes;
    private readonly IRepository<WoTerminalLine> _terminalLines;
    private readonly IRepository<WoParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly INotificationService _notificationService;

    public WoHeaderService(
        IRepository<WoHeader> headers,
        IRepository<WoLine> lines,
        IRepository<WoImportLine> importLines,
        IRepository<WoLineSerial> lineSerials,
        IRepository<WoRoute> routes,
        IRepository<WoTerminalLine> terminalLines,
        IRepository<WoParameter> parameters,
        IRepository<Notification> notifications,
        IRepository<PHeader> packageHeaders,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor,
        IErpReadEnrichmentService erpReadEnrichmentService,
        INotificationService notificationService)
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
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _notificationService = notificationService;
    }

    public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var entities = await _headers.Query().Where(x => x.BranchCode == branchCode).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoHeaderDto>>(entities);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
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
        var dtos = _mapper.Map<List<WoHeaderDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<WoHeaderDto>>.SuccessResult(new PagedResponse<WoHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoHeaderNotFound");
            return ApiResponse<WoHeaderDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WoHeaderDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoHeaderDto>> CreateAsync(CreateWoHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WoHeader>(createDto) ?? new WoHeader();
        entity.BranchCode = string.IsNullOrWhiteSpace(createDto.BranchCode) ? (_currentUserAccessor.BranchCode ?? "0") : createDto.BranchCode;
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<WoHeaderDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WoHeaderDto>> UpdateAsync(long id, UpdateWoHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoHeaderNotFound");
            return ApiResponse<WoHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<WoHeaderDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var hasImportLines = await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken);
        if (hasImportLines)
        {
            var msg = _localizationService.GetLocalizedString("WoHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var notFound = _localizationService.GetLocalizedString("WoHeaderNotFound");
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
                .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.WO)
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
                    Title = _localizationService.GetLocalizedString("WoDoneNotificationTitle", orderNumber),
                    Message = _localizationService.GetLocalizedString("WoDoneNotificationMessage", orderNumber),
                    TitleKey = "WoDoneNotificationTitle",
                    MessageKey = "WoDoneNotificationMessage",
                    Channel = NotificationChannel.Web,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = entity.CreatedBy.Value,
                    RelatedEntityType = NotificationEntityType.WODone,
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

            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoHeaderCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<WoHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var entities = await _headers.Query()
            .Where(h => !h.IsCompleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<WoHeaderDto>>(entities);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<WoHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var lines = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var lineIds = lines.Select(x => x.Id).ToList();

        IEnumerable<WoLineSerial> lineSerials = Array.Empty<WoLineSerial>();
        if (lineIds.Count > 0)
        {
            lineSerials = await _lineSerials.Query().Where(x => lineIds.Contains(x.LineId)).ToListAsync(cancellationToken);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();

        IEnumerable<WoRoute> routes = Array.Empty<WoRoute>();
        if (importLineIds.Count > 0)
        {
            routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);
        }

        var lineDtos = _mapper.Map<List<WoLineDto>>(lines);
        if (lineDtos.Count > 0)
        {
            lineDtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(lineDtos, cancellationToken)).Data?.ToList() ?? lineDtos;
        }
        var importLineDtos = _mapper.Map<List<WoImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
            importLineDtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(importLineDtos, cancellationToken)).Data?.ToList() ?? importLineDtos;
        }

        var dto = new WoAssignedOrderLinesDto
        {
            Lines = lineDtos,
            LineSerials = _mapper.Map<List<WoLineSerialDto>>(lineSerials),
            ImportLines = importLineDtos,
            Routes = _mapper.Map<List<WoRouteDto>>(routes)
        };

        return ApiResponse<WoAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderAssignedOrderLinesRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoHeaderDto>> GenerateWarehouseOutboundOrderAsync(GenerateWarehouseOutboundOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<WoHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<WoHeader>(request.Header) ?? new WoHeader();
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();

            if (request.Lines?.Count > 0)
            {
                var lines = new List<WoLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<WoLine>(lineDto) ?? new WoLine();
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
                var serials = new List<WoLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<WoHeaderDto>("WoHeaderCreationError", "LineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<WoHeaderDto>("WoHeaderCreationError", "LineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<WoHeaderDto>("WoHeaderCreationError", "LineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<WoLineSerial>(serialDto) ?? new WoLineSerial();
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
                        var terminalLine = _mapper.Map<WoTerminalLine>(x) ?? new WoTerminalLine();
                        terminalLine.HeaderId = header.Id;
                        return terminalLine;
                    })
                    .ToList();

                await _terminalLines.AddRangeAsync(terminalLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                    terminalLines,
                    header.Id.ToString(),
                    NotificationEntityType.WOHeader,
                    "WO_HEADER",
                    "WoHeaderNotificationTitle",
                    "WoHeaderNotificationMessage",
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

            var dto = _mapper.Map<WoHeaderDto>(header);
            return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<int>> BulkCreateWarehouseOutboundAsync(BulkCreateWoRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            return ApiResponse<int>.ErrorResult(
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
            var header = _mapper.Map<WoHeader>(request.Header) ?? new WoHeader();
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (header.Id <= 0)
            {
                return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "HeaderInsertFailed", 500, cancellationToken);
            }

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var lineGuidToId = new Dictionary<Guid, long>();
            if (request.Lines?.Count > 0)
            {
                var lines = new List<WoLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<WoLine>(lineDto) ?? new WoLine();
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
                var serials = new List<WoLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    long lineId;
                    if (serialDto.LineGroupGuid.HasValue)
                    {
                        if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out lineId))
                        {
                            return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "LineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(serialDto.LineClientKey!, out lineId))
                        {
                            return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "LineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "LineReferenceMissing", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<WoLineSerial>(serialDto) ?? new WoLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.ImportLines?.Count > 0)
            {
                var importLines = new List<WoImportLine>(request.ImportLines.Count);
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

                    var importLine = new WoImportLine
                    {
                        HeaderId = header.Id,
                        LineId = lineId,
                        StockCode = importDto.StockCode,
                        Description = importDto.ErpOrderNo ?? importDto.ErpOrderNumber,
                        Description1 = importDto.ScannedBarkod,
                        Description2 = importDto.ErpOrderLineNumber
                    };
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
                var routes = new List<WoRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    long importLineId;
                    if (routeDto.ImportLineGroupGuid.HasValue)
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineGroupGuid.Value.ToString(), out importLineId))
                        {
                            return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "ImportLineGroupGuidNotFound", 400, cancellationToken);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                    {
                        if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey!, out importLineId))
                        {
                            return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "ImportLineClientKeyNotFound", 400, cancellationToken);
                        }
                    }
                    else
                    {
                        return await RollbackWithErrorAsync<int>("WoHeaderCreationError", "ImportLineReferenceMissing", 400, cancellationToken);
                    }

                    var route = new WoRoute
                    {
                        ImportLineId = importLineId,
                        Quantity = routeDto.Quantity,
                        SerialNo = routeDto.SerialNo,
                        SerialNo2 = routeDto.SerialNo2,
                        SourceWarehouse = routeDto.SourceWarehouse.HasValue ? (int?)routeDto.SourceWarehouse.Value : null,
                        TargetWarehouse = routeDto.TargetWarehouse.HasValue ? (int?)routeDto.TargetWarehouse.Value : null,
                        SourceCellCode = routeDto.SourceCellCode,
                        TargetCellCode = routeDto.TargetCellCode,
                        Description = routeDto.Description
                    };
                    routes.Add(route);
                }

                await _routes.AddRangeAsync(routes, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<int>.SuccessResult(1, _localizationService.GetLocalizedString("WoHeaderBulkCreateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PagedResponse<WoHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoHeaderDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;

        var result = new PagedResponse<WoHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<WoHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WoHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("WoHeaderNotFound");
            return ApiResponse<WoHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
        {
            var message = _localizationService.GetLocalizedString("WoHeaderApprovalUpdateError");
            return ApiResponse<WoHeaderDto>.ErrorResult(message, message, 400);
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

        var dto = _mapper.Map<WoHeaderDto>(entity);
        return ApiResponse<WoHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoHeaderApprovalUpdatedSuccessfully"));
    }

    private async Task<ApiResponse<bool>?> ValidateLineSerialVsRouteQuantitiesAsync(long headerId, WoParameter? parameter, CancellationToken cancellationToken)
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
            var totalLineSerialQuantity = await _lineSerials.Query()
                .Where(ls => ls.LineId == line.Id)
                .SumAsync(ls => ls.Quantity, cancellationToken);

            var totalRouteQuantity = await _routes.Query()
                .Where(r => r.ImportLine.LineId == line.Id && !r.ImportLine.IsDeleted)
                .SumAsync(r => r.Quantity, cancellationToken);

            if (requireAllOrderItemsCollected)
            {
                if (totalRouteQuantity <= 0.000001m)
                {
                    var msg = _localizationService.GetLocalizedString("WoHeaderAllOrderItemsMustBeCollected", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty);
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
            }
            else if (totalRouteQuantity <= 0.000001m)
            {
                continue;
            }

            var allowLess = parameter?.AllowLessQuantityBasedOnOrder ?? false;
            var allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;

            if (!allowLess && !allowMore && Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString("WoHeaderQuantityExactMatchRequired", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (allowLess && !allowMore && totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString("WoHeaderQuantityCannotBeGreater", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (!allowLess && allowMore && totalRouteQuantity + 0.000001m < totalLineSerialQuantity)
            {
                var msg = _localizationService.GetLocalizedString("WoHeaderQuantityCannotBeLess", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }
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

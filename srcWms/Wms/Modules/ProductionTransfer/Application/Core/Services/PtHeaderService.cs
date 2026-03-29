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
    private readonly IRepository<PtParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly INotificationService _notificationService;

    public PtHeaderService(
        IRepository<PtHeader> headers,
        IRepository<PtLine> lines,
        IRepository<PtImportLine> importLines,
        IRepository<PtLineSerial> lineSerials,
        IRepository<PtRoute> routes,
        IRepository<PtTerminalLine> terminalLines,
        IRepository<PtParameter> parameters,
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

    public async Task<ApiResponse<IEnumerable<PtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var entities = await _headers.Query().Where(x => x.BranchCode == branchCode).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtHeaderDto>>(entities);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
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
        var dtos = _mapper.Map<List<PtHeaderDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
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
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtHeaderDto>> CreateAsync(CreatePtHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtHeader>(createDto) ?? new PtHeader();
        entity.BranchCode = string.IsNullOrWhiteSpace(createDto.BranchCode) ? (_currentUserAccessor.BranchCode ?? "0") : createDto.BranchCode;
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PtHeaderDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
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
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PtHeaderDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
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
            .Where(h => !h.IsCompleted && h.BranchCode == branchCode && terminalLineHeaderIds.Contains(h.Id))
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<PtHeaderDto>>(entities);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtHeaderAssignedOrdersRetrievedSuccessfully"));
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
            lineDtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(lineDtos, cancellationToken)).Data?.ToList() ?? lineDtos;
        }
        var importLineDtos = _mapper.Map<List<PtImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
            importLineDtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(importLineDtos, cancellationToken)).Data?.ToList() ?? importLineDtos;
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
                        SourceWarehouse = routeDto.SourceWarehouse,
                        TargetWarehouse = routeDto.TargetWarehouse,
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
            .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtHeaderDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateCustomerNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;

        var result = new PagedResponse<PtHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<PtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
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

            if (!allowLess && !allowMore && Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString("PtHeaderQuantityExactMatchRequired", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (allowLess && !allowMore && totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString("PtHeaderQuantityCannotBeGreater", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (!allowLess && allowMore && totalRouteQuantity + 0.000001m < totalLineSerialQuantity)
            {
                var msg = _localizationService.GetLocalizedString("PtHeaderQuantityCannotBeLess", line.Id, line.StockCode ?? string.Empty, line.YapKod ?? string.Empty, totalLineSerialQuantity, totalRouteQuantity);
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

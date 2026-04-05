using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Domain.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Communications.Enums;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrHeaderService : IGrHeaderService
{
    private readonly IRepository<GrHeader> _headers;
    private readonly IRepository<GrLine> _lines;
    private readonly IRepository<GrImportLine> _importLines;
    private readonly IRepository<GrLineSerial> _lineSerials;
    private readonly IRepository<GrRoute> _routes;
    private readonly IRepository<GrTerminalLine> _terminalLines;
    private readonly IRepository<GrImportDocument> _importDocuments;
    private readonly IRepository<GrParameter> _parameters;
    private readonly IRepository<Notification> _notifications;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GrHeaderService(
        IRepository<GrHeader> headers,
        IRepository<GrLine> lines,
        IRepository<GrImportLine> importLines,
        IRepository<GrLineSerial> lineSerials,
        IRepository<GrRoute> routes,
        IRepository<GrTerminalLine> terminalLines,
        IRepository<GrImportDocument> importDocuments,
        IRepository<GrParameter> parameters,
        IRepository<Notification> notifications,
        IRepository<PHeader> packageHeaders,
        ICurrentUserAccessor currentUserAccessor,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IEntityReferenceResolver entityReferenceResolver,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _headers = headers;
        _lines = lines;
        _importLines = importLines;
        _lineSerials = lineSerials;
        _routes = routes;
        _terminalLines = terminalLines;
        _importDocuments = importDocuments;
        _parameters = parameters;
        _notifications = notifications;
        _packageHeaders = packageHeaders;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _entityReferenceResolver = entityReferenceResolver;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var query = _headers.Query()
            .Where(x => x.BranchCode == branchCode)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrHeaderDto>>(items);

        var result = new PagedResponse<GrHeaderDto>(dtos, request.PageNumber < 1 ? totalCount : totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var items = await _headers.Query().Where(x => x.BranchCode == branchCode).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrHeaderDto>>(items);
        return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrHeaderDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var nf = _localizationService.GetLocalizedString("GrHeaderNotFound");
            return ApiResponse<GrHeaderDto>.ErrorResult(nf, nf, 404);
        }

        var dto = _mapper.Map<GrHeaderDto>(entity);
        return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrHeaderDto>> CreateAsync(CreateGrHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        if (createDto == null)
        {
            return ApiResponse<GrHeaderDto>.ErrorResult(
                _localizationService.GetLocalizedString("InvalidModelState"),
                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                400);
        }

        if (string.IsNullOrWhiteSpace(createDto.BranchCode))
        {
            createDto.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.CustomerCode))
        {
            return ApiResponse<GrHeaderDto>.ErrorResult(
                _localizationService.GetLocalizedString("InvalidModelState"),
                _localizationService.GetLocalizedString("HeaderFieldsMissing"),
                400);
        }

        var entity = _mapper.Map<GrHeader>(createDto) ?? new GrHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<GrHeaderDto>(entity);
        return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<long>> BulkCreateAsync(BulkCreateGrRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        if (string.IsNullOrWhiteSpace(request.Header.CustomerCode))
        {
            var message = _localizationService.GetLocalizedString("HeaderFieldsMissing");
            return ApiResponse<long>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<GrHeader>(request.Header) ?? new GrHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            var parameter = await _parameters.Query().FirstOrDefaultAsync(cancellationToken);
            header.IsPendingApproval = parameter?.RequireApprovalBeforeErp == true;

            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.Documents?.Count > 0)
            {
                var documents = request.Documents
                    .Where(x => x?.Base64 != null)
                    .Select(x =>
                    {
                        var document = _mapper.Map<GrImportDocument>(x);
                        document.HeaderId = header.Id;
                        return document;
                    })
                    .ToList();

                if (documents.Count != request.Documents.Count)
                {
                    return await RollbackWithErrorAsync<long>("InvalidModelState", "InvalidModelState", 400, cancellationToken);
                }

                await _importDocuments.AddRangeAsync(documents, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.Lines?.Count > 0)
            {
                var lines = new List<GrLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    if (string.IsNullOrWhiteSpace(lineDto.ClientKey))
                    {
                        return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "LineClientKeyMissing", 400, cancellationToken);
                    }

                    var line = _mapper.Map<GrLine>(lineDto) ?? new GrLine();
                    await _entityReferenceResolver.ResolveAsync(line, cancellationToken);
                    line.HeaderId = header.Id;
                    lines.Add(line);
                }

                await _lines.AddRangeAsync(lines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                for (var i = 0; i < request.Lines.Count; i++)
                {
                    lineKeyToId[request.Lines[i].ClientKey] = lines[i].Id;
                }
            }

            if (request.SerialLines?.Count > 0)
            {
                var serials = new List<GrLineSerial>(request.SerialLines.Count);
                foreach (var serialDto in request.SerialLines)
                {
                    if (string.IsNullOrWhiteSpace(serialDto.ImportLineClientKey))
                    {
                        return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "ImportLineClientKeyMissing", 400, cancellationToken);
                    }

                    serials.Add(_mapper.Map<GrLineSerial>(serialDto) ?? new GrLineSerial());
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.ImportLines?.Count > 0)
            {
                var importLines = new List<GrImportLine>(request.ImportLines.Count);
                foreach (var importDto in request.ImportLines)
                {
                    if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                    {
                        return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "ImportLineClientKeyMissing", 400, cancellationToken);
                    }

                    long? lineId = null;
                    if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                    {
                        if (!lineKeyToId.TryGetValue(importDto.LineClientKey, out var foundLineId))
                        {
                            return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "LineClientKeyNotFound", 400, cancellationToken);
                        }

                        lineId = foundLineId;
                    }

                    var importLine = _mapper.Map<GrImportLine>(importDto) ?? new GrImportLine();
                    await _entityReferenceResolver.ResolveAsync(importLine, cancellationToken);
                    importLine.HeaderId = header.Id;
                    importLine.LineId = lineId;
                    importLines.Add(importLine);
                }

                await _importLines.AddRangeAsync(importLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                for (var i = 0; i < request.ImportLines.Count; i++)
                {
                    importLineKeyToId[request.ImportLines[i].ClientKey] = importLines[i].Id;
                }
            }

            if (request.Routes?.Count > 0)
            {
                var routes = new List<GrRoute>(request.Routes.Count);
                foreach (var routeDto in request.Routes)
                {
                    if (string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                    {
                        return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "ImportLineClientKeyMissing", 400, cancellationToken);
                    }

                    if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey, out var importLineId))
                    {
                        return await RollbackWithErrorAsync<long>("InvalidCorrelationKey", "ImportLineClientKeyNotFound", 400, cancellationToken);
                    }

                    var route = _mapper.Map<GrRoute>(routeDto) ?? new GrRoute();
                    route.ImportLineId = importLineId;
                    routes.Add(route);
                }

                await _routes.AddRangeAsync(routes, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<long>.SuccessResult(header.Id, _localizationService.GetLocalizedString("GrHeaderCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<bool>> CompleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("GrHeaderNotFound");
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
                .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.GR)
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
                    Title = _localizationService.GetLocalizedString("GrDoneNotificationTitle", orderNumber),
                    Message = _localizationService.GetLocalizedString("GrDoneNotificationMessage", orderNumber),
                    TitleKey = "GrDoneNotificationTitle",
                    MessageKey = "GrDoneNotificationMessage",
                    Channel = NotificationChannel.Web,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = entity.CreatedBy.Value,
                    RelatedEntityType = NotificationEntityType.GRDone,
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

            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrHeaderCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<GrHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var items = await _headers.Query().Where(x => x.CustomerCode == customerCode && x.BranchCode == branchCode).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrHeaderDto>>(items);
        return ApiResponse<IEnumerable<GrHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrHeaderDto>> GenerateGoodReceiptOrderAsync(GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request?.Header == null)
        {
            var message = _localizationService.GetLocalizedString("RequestOrHeaderMissing");
            return ApiResponse<GrHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), message, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Header.BranchCode))
        {
            request.Header.BranchCode = _currentUserAccessor.BranchCode ?? "0";
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var header = _mapper.Map<GrHeader>(request.Header) ?? new GrHeader();
            await _entityReferenceResolver.ResolveAsync(header, cancellationToken);
            await _headers.AddAsync(header, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (request.Lines?.Count > 0)
            {
                var lines = new List<GrLine>(request.Lines.Count);
                foreach (var lineDto in request.Lines)
                {
                    var line = _mapper.Map<GrLine>(lineDto) ?? new GrLine();
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
                        lineKeyToId[request.Lines[i].ClientKey] = lines[i].Id;
                    }
                }
            }

            if (request.LineSerials?.Count > 0)
            {
                var serials = new List<GrLineSerial>(request.LineSerials.Count);
                foreach (var serialDto in request.LineSerials)
                {
                    if (string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                    {
                        return await RollbackWithErrorAsync<GrHeaderDto>("GrHeaderInvalidCorrelationKey", "GrHeaderLineReferenceMissing", 400, cancellationToken);
                    }

                    if (!lineKeyToId.TryGetValue(serialDto.LineClientKey, out var lineId))
                    {
                        return await RollbackWithErrorAsync<GrHeaderDto>("GrHeaderInvalidCorrelationKey", "GrHeaderLineClientKeyNotFound", 400, cancellationToken);
                    }

                    var serial = _mapper.Map<GrLineSerial>(serialDto) ?? new GrLineSerial();
                    serial.LineId = lineId;
                    serials.Add(serial);
                }

                await _lineSerials.AddRangeAsync(serials, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            List<Notification> createdNotifications = new();
            if (request.TerminalLines?.Count > 0)
            {
                var terminalLines = request.TerminalLines
                    .Select(x =>
                    {
                        var terminalLine = _mapper.Map<GrTerminalLine>(x) ?? new GrTerminalLine();
                        terminalLine.HeaderId = header.Id;
                        return terminalLine;
                    })
                    .ToList();

                await _terminalLines.AddRangeAsync(terminalLines, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                    terminalLines,
                    header.Id.ToString(),
                    NotificationEntityType.GRHeader,
                    "GR_HEADER",
                    "GrHeaderNotificationTitle",
                    "GrHeaderNotificationMessage",
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

            var dto = _mapper.Map<GrHeaderDto>(header);
            return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderGenerateCompletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetAssignedOrdersPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var branchCode = _currentUserAccessor.BranchCode ?? "0";
        var terminalLineHeaderIds = _terminalLines.Query(false, false)
            .Where(t => t.TerminalUserId == userId)
            .Select(t => t.HeaderId);

        var query = _headers.Query()
            .Where(h => !h.IsCompleted
                && h.BranchCode == branchCode
                && terminalLineHeaderIds.Contains(h.Id))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrHeaderDto>>(items);

        var result = new PagedResponse<GrHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrHeaderAssignedOrdersRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var lines = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var lineIds = lines.Select(x => x.Id).ToList();

        IEnumerable<GrLineSerial> lineSerials = Array.Empty<GrLineSerial>();
        if (lineIds.Count > 0)
        {
            lineSerials = await _lineSerials.Query().Where(x => x.LineId.HasValue && lineIds.Contains(x.LineId.Value)).ToListAsync(cancellationToken);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();

        IEnumerable<GrRoute> routes = Array.Empty<GrRoute>();
        if (importLineIds.Count > 0)
        {
            routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);
        }

        var lineDtos = _mapper.Map<List<GrLineDto>>(lines);
        if (lineDtos.Count > 0)
        {
        }
        var importLineDtos = _mapper.Map<List<GrImportLineDto>>(importLines);
        if (importLineDtos.Count > 0)
        {
        }

        var dto = new GrAssignedOrderLinesDto
        {
            Lines = lineDtos,
            LineSerials = _mapper.Map<List<GrLineSerialDto>>(lineSerials),
            ImportLines = importLineDtos,
            Routes = _mapper.Map<List<GrRouteDto>>(routes)
        };

        return ApiResponse<GrAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderAssignedOrderLinesRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrHeaderDto>>(items);

        var result = new PagedResponse<GrHeaderDto>(dtos, totalCount, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<GrHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString("GrHeaderNotFound");
            return ApiResponse<GrHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
        {
            var message = _localizationService.GetLocalizedString("GrHeaderApprovalUpdateError");
            return ApiResponse<GrHeaderDto>.ErrorResult(message, message, 400);
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

        var dto = _mapper.Map<GrHeaderDto>(entity);
        return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderApprovalUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _headers.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var notFound = _localizationService.GetLocalizedString("GrHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
        }

        var importLinesExist = await _importLines.Query()
            .Where(x => x.HeaderId == id)
            .AnyAsync(cancellationToken);

        if (importLinesExist)
        {
            var message = _localizationService.GetLocalizedString("GrHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(message, message, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<GrHeaderDto>> UpdateAsync(int id, UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var notFound = _localizationService.GetLocalizedString("GrHeaderNotFound");
            return ApiResponse<GrHeaderDto>.ErrorResult(notFound, notFound, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<GrHeaderDto>(entity);
        return ApiResponse<GrHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrHeaderUpdatedSuccessfully"));
    }

    private async Task<ApiResponse<T>> RollbackWithErrorAsync<T>(string titleKey, string messageKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        return ApiResponse<T>.ErrorResult(
            _localizationService.GetLocalizedString(titleKey),
            _localizationService.GetLocalizedString(messageKey),
            statusCode);
    }

    private async Task<ApiResponse<bool>?> ValidateLineSerialVsRouteQuantitiesAsync(long headerId, GrParameter? parameter, CancellationToken cancellationToken)
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
                    var msg = _localizationService.GetLocalizedString(
                        "GrHeaderAllOrderItemsMustBeCollected",
                        line.Id,
                        line.StockCode ?? string.Empty,
                        line.YapKod ?? string.Empty);

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
                var msg = _localizationService.GetLocalizedString(
                    "GrHeaderQuantityExactMatchRequired",
                    line.Id,
                    line.StockCode ?? string.Empty,
                    line.YapKod ?? string.Empty,
                    totalLineSerialQuantity,
                    totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (allowLess && !allowMore && totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString(
                    "GrHeaderQuantityCannotBeGreater",
                    line.Id,
                    line.StockCode ?? string.Empty,
                    line.YapKod ?? string.Empty,
                    totalLineSerialQuantity,
                    totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }

            if (!allowLess && allowMore && totalRouteQuantity + 0.000001m < totalLineSerialQuantity)
            {
                var msg = _localizationService.GetLocalizedString(
                    "GrHeaderQuantityCannotBeLess",
                    line.Id,
                    line.StockCode ?? string.Empty,
                    line.YapKod ?? string.Empty,
                    totalLineSerialQuantity,
                    totalRouteQuantity);
                return ApiResponse<bool>.ErrorResult(msg, msg, 400);
            }
        }

        return null;
    }
}

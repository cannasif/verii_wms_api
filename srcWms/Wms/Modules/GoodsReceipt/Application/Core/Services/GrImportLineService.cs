using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrImportLineService : IGrImportLineService
{
    private readonly IRepository<GrImportLine> _importLines;
    private readonly IRepository<GrHeader> _headers;
    private readonly IRepository<GrLine> _lines;
    private readonly IRepository<GrLineSerial> _lineSerials;
    private readonly IRepository<GrRoute> _routes;
    private readonly IRepository<GrParameter> _parameters;
    private readonly IRepository<PLine> _packageLines;
    private readonly IRepository<PPackage> _packages;
    private readonly IRepository<PHeader> _packageHeaders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IAssignedBarcodeMatchingService _assignedBarcodeMatchingService;
    private readonly IMapper _mapper;

    public GrImportLineService(
        IRepository<GrImportLine> importLines,
        IRepository<GrHeader> headers,
        IRepository<GrLine> lines,
        IRepository<GrLineSerial> lineSerials,
        IRepository<GrRoute> routes,
        IRepository<GrParameter> parameters,
        IRepository<PLine> packageLines,
        IRepository<PPackage> packages,
        IRepository<PHeader> packageHeaders,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IAssignedBarcodeMatchingService assignedBarcodeMatchingService,
        IMapper mapper)
    {
        _importLines = importLines;
        _headers = headers;
        _lines = lines;
        _lineSerials = lineSerials;
        _routes = routes;
        _parameters = parameters;
        _packageLines = packageLines;
        _packages = packages;
        _packageHeaders = packageHeaders;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _assignedBarcodeMatchingService = assignedBarcodeMatchingService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<GrImportLineDto>>.SuccessResult(new PagedResponse<GrImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("GrImportLineNotFound");
            return ApiResponse<GrImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<GrImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<GrImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetWithRoutesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        return await BuildImportLinesWithRoutesAsync(headerId, cancellationToken);
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrImportLine>(createDto) ?? new GrImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GrImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<GrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportLineDto>> UpdateAsync(long id, UpdateGrImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("GrImportLineNotFound");
            return ApiResponse<GrImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GrImportLineDto>(entity);
        return ApiResponse<GrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("GrImportLineRoutesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var hasActiveLineSerials = entity.LineId.HasValue && await _lineSerials.Query()
            .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId.Value)
            .AnyAsync(cancellationToken);
        if (hasActiveLineSerials)
        {
            var msg = _localizationService.GetLocalizedString("GrImportLineLineSerialsExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _importLines.SoftDelete(id, cancellationToken);
            var headerId = entity.HeaderId;
            var hasOtherLines = await _lines.Query().Where(l => !l.IsDeleted && l.HeaderId == headerId).AnyAsync(cancellationToken);
            var hasOtherImportLines = await _importLines.Query().Where(il => !il.IsDeleted && il.HeaderId == headerId).AnyAsync(cancellationToken);
            if (!hasOtherLines && !hasOtherImportLines)
            {
                await _headers.SoftDelete(headerId, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<GrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddGrImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Quantity <= 0)
            {
                return await RollbackImportLineErrorAsync<GrImportLineDto>("GrImportLineQuantityInvalid", "GrImportLineQuantityInvalid", 400, cancellationToken);
            }

            var header = await _headers.GetByIdAsync(request.HeaderId, cancellationToken);
            if (header == null || header.IsDeleted)
            {
                return await RollbackImportLineErrorAsync<GrImportLineDto>("GrHeaderNotFound", "GrHeaderNotFound", 404, cancellationToken);
            }

            var parameter = await _parameters.Query().Where(p => !p.IsDeleted).FirstOrDefaultAsync(cancellationToken);
            var lines = await _lines.Query()
                .Where(line => line.HeaderId == request.HeaderId && !line.IsDeleted)
                .ToListAsync(cancellationToken);
            var lineIds = lines.Select(line => line.Id).ToList();
            var lineSerials = await _lineSerials.Query()
                .Where(serial => !serial.IsDeleted && serial.LineId.HasValue && lineIds.Contains(serial.LineId.Value))
                .ToListAsync(cancellationToken);
            var existingRoutes = await _routes.Query()
                .Where(route => !route.IsDeleted
                    && !route.ImportLine.IsDeleted
                    && route.ImportLine.HeaderId == request.HeaderId)
                .Select(route => new AssignedBarcodeRouteSnapshot
                {
                    LineId = route.ImportLine.LineId,
                    ScannedBarcode = route.ScannedBarcode,
                    SerialNo = route.SerialNo,
                    Quantity = route.Quantity,
                    SourceCellCode = route.SourceCellCode,
                    TargetCellCode = route.TargetCellCode
                })
                .ToListAsync(cancellationToken);

            var matchResult = await _assignedBarcodeMatchingService.MatchAsync(new AssignedBarcodeMatchRequest<GrLine, GrLineSerial>
            {
                BarcodeRequest = new ResolveBarcodeRequestDto
                {
                    ModuleKey = BarcodeModuleKeys.GoodsReceiptAssigned,
                    Barcode = request.Barcode,
                    FallbackStockCode = request.StockCode,
                    FallbackStockName = request.StockName,
                    FallbackYapKod = request.YapKod,
                    FallbackYapAcik = request.YapAcik,
                    FallbackSerialNumber = request.SerialNo
                },
                RequestQuantity = request.Quantity,
                RawBarcode = request.Barcode,
                SourceCellCode = request.SourceCellCode,
                TargetCellCode = request.TargetCellCode,
                AllowMoreQuantityBasedOnOrder = parameter?.AllowMoreQuantityBasedOnOrder ?? false,
                Lines = lines,
                LineSerials = lineSerials,
                ExistingRoutes = existingRoutes,
                LineIdSelector = line => line.Id,
                LineSerialLineIdSelector = serial => serial.LineId,
                StockAndYapKodNotMatchedErrorCode = "GrImportLineStokCodeAndYapCodeNotMatch",
                SerialNotMatchedErrorCode = "GrImportLineSerialNotMatch",
                NoMatchingLineErrorCode = "GrImportLineNoMatchingLine",
                QuantityExceededErrorCode = "GrHeaderQuantityCannotBeGreater"
            }, cancellationToken);

            if (!matchResult.Success)
            {
                var errorCode = matchResult.ErrorCode ?? "BarcodeCouldNotBeResolved";
                return await RollbackImportLineErrorAsync<GrImportLineDto>(errorCode, errorCode, matchResult.StatusCode ?? 400, cancellationToken, matchResult.Details);
            }

            var reqStock = matchResult.RequestedStockCode;
            var reqYap = matchResult.RequestedYapKod;
            var serialNo = matchResult.RequestedSerialNo ?? string.Empty;
            var selectedLineId = matchResult.SelectedLineId!.Value;

            var importLine = await _importLines.Query(tracking: true)
                .Where(il => il.HeaderId == request.HeaderId
                    && il.LineId == selectedLineId
                    && (il.StockCode ?? string.Empty).Trim() == reqStock
                    && (il.YapKod ?? string.Empty).Trim() == (reqYap ?? string.Empty)
                    && !il.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (importLine == null)
            {
                importLine = new GrImportLine
                {
                    HeaderId = request.HeaderId,
                    LineId = selectedLineId,
                    StockCode = reqStock,
                    YapKod = string.IsNullOrWhiteSpace(reqYap) ? null : reqYap,
                    CreatedDate = DateTimeProvider.Now
                };
                await _importLines.AddAsync(importLine, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var route = new GrRoute
            {
                ImportLineId = importLine.Id,
                ScannedBarcode = request.Barcode,
                Quantity = request.Quantity,
                SerialNo = string.IsNullOrWhiteSpace(serialNo) ? null : serialNo,
                SerialNo2 = request.SerialNo2,
                SerialNo3 = request.SerialNo3,
                SerialNo4 = request.SerialNo4,
                SourceCellCode = request.SourceCellCode,
                TargetCellCode = request.TargetCellCode,
                CreatedDate = DateTimeProvider.Now
            };

            await _routes.AddAsync(route, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var dto = _mapper.Map<GrImportLineDto>(importLine);
            return ApiResponse<GrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        return await BuildImportLinesWithRoutesAsync(headerId, cancellationToken);
    }

    private async Task<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>> BuildImportLinesWithRoutesAsync(long headerId, CancellationToken cancellationToken)
    {
        var header = await _headers.GetByIdAsync(headerId, cancellationToken);
        if (header == null || header.IsDeleted)
        {
            return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("GrHeaderNotFound"), _localizationService.GetLocalizedString("GrHeaderNotFound"), 404);
        }

        var importLines = await _importLines.Query()
            .Where(x => x.HeaderId == headerId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!importLines.Any())
        {
            return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.SuccessResult(Array.Empty<GrImportLineWithRoutesDto>(), _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
        }

        var importLineIds = importLines.Select(il => il.Id).ToList();
        var routes = await _routes.Query()
            .Where(r => importLineIds.Contains(r.ImportLineId) && !r.IsDeleted)
            .ToListAsync(cancellationToken);

        var routesByImportLineId = routes.GroupBy(r => r.ImportLineId).ToDictionary(g => g.Key, g => g.ToList());
        var packageInfoByRouteId = new Dictionary<long, (long? PackageLineId, string? PackageNo, long? PackageHeaderId)>();
        if (routes.Count > 0)
        {
            var routeIds = routes.Select(r => r.Id).ToList();
            var packageInfo = await (
                from pl in _packageLines.Query()
                join p in _packages.Query() on pl.PackageId equals p.Id
                join ph in _packageHeaders.Query() on p.PackingHeaderId equals ph.Id
                where pl.SourceRouteId.HasValue
                    && routeIds.Contains(pl.SourceRouteId.Value)
                    && ph.SourceHeaderId == headerId
                    && ph.SourceType == PHeaderSourceType.GR
                select new
                {
                    RouteId = pl.SourceRouteId!.Value,
                    PackageLineId = (long?)pl.Id,
                    p.PackageNo,
                    PackageHeaderId = (long?)ph.Id
                })
                .ToListAsync(cancellationToken);

            packageInfoByRouteId = packageInfo
                .GroupBy(x => x.RouteId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var first = g.First();
                        return (first.PackageLineId, (string?)first.PackageNo, first.PackageHeaderId);
                    });
        }

        var items = importLines.Select(importLine =>
        {
            var importLineDto = _mapper.Map<GrImportLineDto>(importLine);
            var routeDtos = routesByImportLineId.GetValueOrDefault(importLine.Id, new List<GrRoute>())
                .Select(route =>
                {
                    var routeDto = _mapper.Map<GrRouteDto>(route);
                    if (packageInfoByRouteId.TryGetValue(route.Id, out var packageInfo))
                    {
                        routeDto.PackageLineId = packageInfo.PackageLineId;
                        routeDto.PackageNo = packageInfo.PackageNo;
                        routeDto.PackageHeaderId = packageInfo.PackageHeaderId;
                    }

                    return routeDto;
                })
                .ToList();

            return new GrImportLineWithRoutesDto
            {
                ImportLine = importLineDto,
                Routes = routeDtos
            };
        }).ToList();

        return ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    private async Task<ApiResponse<T>> RollbackImportLineErrorAsync<T>(string titleKey, string messageKey, int statusCode, CancellationToken cancellationToken, object? details = null)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        return ApiResponse<T>.ErrorResult(_localizationService.GetLocalizedString(titleKey), _localizationService.GetLocalizedString(messageKey), statusCode);
    }
}

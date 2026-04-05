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
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
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
        return ApiResponse<GrImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrImportLineDto>>(items);
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
        return ApiResponse<IEnumerable<GrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportLineDto>> CreateAsync(CreateGrImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrImportLine>(createDto) ?? new GrImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GrImportLineDto>(entity);
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

            var reqStock = (request.StockCode ?? string.Empty).Trim();
            var reqYap = (request.YapKod ?? string.Empty).Trim();
            var matchingLines = await _lines.Query()
                .Where(l => l.HeaderId == request.HeaderId
                    && !l.IsDeleted
                    && (l.StockCode ?? string.Empty).Trim() == reqStock
                    && (l.YapKod ?? string.Empty).Trim() == reqYap)
                .ToListAsync(cancellationToken);

            if (!matchingLines.Any())
            {
                return await RollbackImportLineErrorAsync<GrImportLineDto>("GrImportLineStokCodeAndYapCodeNotMatch", "GrImportLineStokCodeAndYapCodeNotMatch", 404, cancellationToken);
            }

            var serialNo = (request.SerialNo ?? string.Empty).Trim();
            var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);
            var lineIds = matchingLines.Select(l => l.Id).ToList();
            var lineSerials = await _lineSerials.Query()
                .Where(ls => !ls.IsDeleted && ls.LineId.HasValue && lineIds.Contains(ls.LineId.Value))
                .ToListAsync(cancellationToken);

            var hasSerialInLineSerials = lineSerials.Any(ls => !string.IsNullOrWhiteSpace(ls.SerialNo));
            var parameter = await _parameters.Query().Where(p => !p.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (hasSerialInLineSerials && hasRequestSerial)
            {
                var matchingLineSerials = lineSerials.Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo).ToList();
                if (!matchingLineSerials.Any())
                {
                    return await RollbackImportLineErrorAsync<GrImportLineDto>("GrImportLineSerialNotMatch", "GrImportLineSerialNotMatch", 404, cancellationToken);
                }

                var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
                var totalRouteQuantity = await _routes.Query()
                    .Where(r => !r.IsDeleted
                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                        && !r.ImportLine.IsDeleted
                        && (r.SerialNo ?? string.Empty).Trim() == serialNo)
                    .SumAsync(r => r.Quantity, cancellationToken);

                var totalRouteAfterAdd = totalRouteQuantity + request.Quantity;
                var allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;
                if (!allowMore && totalRouteAfterAdd > totalLineSerialQuantity + 0.000001m)
                {
                    return await RollbackImportLineErrorAsync<GrImportLineDto>("GrHeaderQuantityCannotBeGreater", "GrHeaderQuantityCannotBeGreater", 400, cancellationToken);
                }
            }
            else
            {
                var totalLineSerialQuantity = lineSerials.Sum(ls => ls.Quantity);
                var totalRouteQuantity = await _routes.Query()
                    .Where(r => !r.IsDeleted
                        && lineIds.Contains(r.ImportLine.LineId ?? 0)
                        && !r.ImportLine.IsDeleted)
                    .SumAsync(r => r.Quantity, cancellationToken);
                var totalRouteAfterAdd = totalRouteQuantity + request.Quantity;
                var allowMore = parameter?.AllowMoreQuantityBasedOnOrder ?? false;
                if (!allowMore && totalRouteAfterAdd > totalLineSerialQuantity + 0.000001m)
                {
                    return await RollbackImportLineErrorAsync<GrImportLineDto>("GrHeaderQuantityCannotBeGreater", "GrHeaderQuantityCannotBeGreater", 400, cancellationToken);
                }
            }

            long? selectedLineId = null;
            if (hasSerialInLineSerials && hasRequestSerial)
            {
                var linesWithSerial = lineSerials
                    .Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo)
                    .Select(ls => ls.LineId)
                    .Distinct()
                    .ToList();
                if (linesWithSerial.Count == 1)
                {
                    selectedLineId = linesWithSerial.First();
                }
            }

            if (!selectedLineId.HasValue)
            {
                var lineQuantities = new List<(long LineId, decimal Remaining)>();
                foreach (var line in matchingLines)
                {
                    var lineSerialTotal = lineSerials.Where(ls => ls.LineId == line.Id).Sum(ls => ls.Quantity);
                    var routeTotal = await _routes.Query()
                        .Where(r => !r.IsDeleted && r.ImportLine.LineId == line.Id && !r.ImportLine.IsDeleted)
                        .SumAsync(r => r.Quantity, cancellationToken);
                    lineQuantities.Add((line.Id, lineSerialTotal - routeTotal));
                }
                var bestLine = lineQuantities.OrderByDescending(x => x.Remaining).FirstOrDefault();
                selectedLineId = bestLine.LineId > 0 ? bestLine.LineId : matchingLines.First().Id;
            }

            if (!selectedLineId.HasValue)
            {
                return await RollbackImportLineErrorAsync<GrImportLineDto>("GrImportLineNoMatchingLine", "GrImportLineNoMatchingLine", 400, cancellationToken);
            }

            var importLine = await _importLines.Query(tracking: true)
                .Where(il => il.HeaderId == request.HeaderId
                    && il.LineId == selectedLineId.Value
                    && (il.StockCode ?? string.Empty).Trim() == reqStock
                    && (il.YapKod ?? string.Empty).Trim() == reqYap
                    && !il.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (importLine == null)
            {
                importLine = new GrImportLine
                {
                    HeaderId = request.HeaderId,
                    LineId = selectedLineId.Value,
                    StockCode = request.StockCode ?? reqStock,
                    YapKod = request.YapKod ?? reqYap,
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
                SerialNo = request.SerialNo,
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

    private async Task<ApiResponse<T>> RollbackImportLineErrorAsync<T>(string titleKey, string messageKey, int statusCode, CancellationToken cancellationToken)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        return ApiResponse<T>.ErrorResult(_localizationService.GetLocalizedString(titleKey), _localizationService.GetLocalizedString(messageKey), statusCode);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Services;

public sealed class ShImportLineService : IShImportLineService
{
    private readonly IRepository<ShImportLine> _importLines;
    private readonly IRepository<ShHeader> _headers;
    private readonly IRepository<ShLine> _lines;
    private readonly IRepository<ShLineSerial> _lineSerials;
    private readonly IRepository<ShRoute> _routes;
    private readonly IRepository<ShParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IAssignedBarcodeMatchingService _assignedBarcodeMatchingService;
    private readonly IMapper _mapper;

    public ShImportLineService(
        IRepository<ShImportLine> importLines,
        IRepository<ShHeader> headers,
        IRepository<ShLine> lines,
        IRepository<ShLineSerial> lineSerials,
        IRepository<ShRoute> routes,
        IRepository<ShParameter> parameters,
        IUnitOfWork unitOfShrk,
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
        _unitOfWork = unitOfShrk;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _assignedBarcodeMatchingService = assignedBarcodeMatchingService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ShImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<ShImportLineDto>>.SuccessResult(new PagedResponse<ShImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("ShImportLineNotFound");
            return ApiResponse<ShImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<ShImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<ShImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<ShImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ShImportLine>(createDto) ?? new ShImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<ShImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("ShImportLineNotFound");
            return ApiResponse<ShImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<ShImportLineDto>(entity);
        return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("ShImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<ShImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddShImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Quantity <= 0)
            {
                return await RollbackImportLineErrorAsync("ShImportLineQuantityInvalid", 400, cancellationToken);
            }

            var header = await _headers.GetByIdAsync(request.HeaderId, cancellationToken);
            if (header == null || header.IsDeleted)
            {
                return await RollbackImportLineErrorAsync("ShHeaderNotFound", 404, cancellationToken);
            }

            var parameter = await _parameters.Query().Where(x => !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
            var lines = await _lines.Query()
                .Where(line => line.HeaderId == request.HeaderId && !line.IsDeleted)
                .ToListAsync(cancellationToken);
            var lineIds = lines.Select(line => line.Id).ToList();
            var lineSerials = await _lineSerials.Query()
                .Where(serial => !serial.IsDeleted && lineIds.Contains(serial.LineId))
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

            var matchResult = await _assignedBarcodeMatchingService.MatchAsync(new AssignedBarcodeMatchRequest<ShLine, ShLineSerial>
            {
                BarcodeRequest = new ResolveBarcodeRequestDto
                {
                    ModuleKey = BarcodeModuleKeys.ShippingAssigned,
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
                StockAndYapKodNotMatchedErrorCode = "ShImportLineStockAndYapKodNotMatched",
                SerialNotMatchedErrorCode = "ShImportLineSerialNotMatched",
                NoMatchingLineErrorCode = "ShImportLineMatchingLineNotFound",
                QuantityExceededErrorCode = "ShHeaderQuantityCannotBeGreater"
            }, cancellationToken);

            if (!matchResult.Success)
            {
                return await RollbackImportLineErrorAsync(matchResult.ErrorCode ?? "BarcodeCouldNotBeResolved", matchResult.StatusCode ?? 400, cancellationToken, matchResult.Details);
            }

            var requestedStockCode = matchResult.RequestedStockCode;
            var requestedYapKod = matchResult.RequestedYapKod;
            var requestSerial = matchResult.RequestedSerialNo ?? string.Empty;
            var selectedLineId = matchResult.SelectedLineId!.Value;

            var importLine = await _importLines.Query(tracking: true)
                .Where(line => line.HeaderId == request.HeaderId
                    && line.LineId == selectedLineId
                    && !line.IsDeleted
                    && (line.StockCode ?? string.Empty).Trim() == requestedStockCode
                    && (line.YapKod ?? string.Empty).Trim() == (requestedYapKod ?? string.Empty))
                .FirstOrDefaultAsync(cancellationToken);

            if (importLine == null)
            {
                importLine = new ShImportLine
                {
                    HeaderId = request.HeaderId,
                    LineId = selectedLineId,
                    StockCode = requestedStockCode,
                    YapKod = string.IsNullOrWhiteSpace(requestedYapKod) ? null : requestedYapKod,
                    CreatedDate = DateTimeProvider.Now
                };

                await _importLines.AddAsync(importLine, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var route = new ShRoute
            {
                ImportLineId = importLine.Id,
                ScannedBarcode = request.Barcode,
                Quantity = request.Quantity,
                SerialNo = string.IsNullOrWhiteSpace(requestSerial) ? null : requestSerial,
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

            var dto = _mapper.Map<ShImportLineDto>(importLine);
            await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
            return ApiResponse<ShImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShImportLineCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<ShImportLineDto>>(importLines);
        var routeDtos = _mapper.Map<List<ShRouteDto>>(routes);

        var result = lineDtos.Select(importLine => new ShImportLineWithRoutesDto
        {
            ImportLine = importLine,
            Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("ShImportLineRetrievedSuccessfully"));
    }

    private async Task<ApiResponse<ShImportLineDto>> RollbackImportLineErrorAsync(string localizationKey, int statusCode, CancellationToken cancellationToken, object? details = null)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(localizationKey);
        return ApiResponse<ShImportLineDto>.ErrorResult(message, message, statusCode, errorCode: localizationKey, details: details);
    }
}

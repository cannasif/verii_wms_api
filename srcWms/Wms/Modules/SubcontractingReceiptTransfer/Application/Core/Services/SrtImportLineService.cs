using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtImportLineService : ISrtImportLineService
{
    private readonly IRepository<SrtImportLine> _importLines;
    private readonly IRepository<SrtHeader> _headers;
    private readonly IRepository<SrtLine> _lines;
    private readonly IRepository<SrtLineSerial> _lineSerials;
    private readonly IRepository<SrtRoute> _routes;
    private readonly IRepository<SrtParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IAssignedBarcodeMatchingService _assignedBarcodeMatchingService;
    private readonly IMapper _mapper;

    public SrtImportLineService(
        IRepository<SrtImportLine> importLines,
        IRepository<SrtHeader> headers,
        IRepository<SrtLine> lines,
        IRepository<SrtLineSerial> lineSerials,
        IRepository<SrtRoute> routes,
        IRepository<SrtParameter> parameters,
        IUnitOfWork unitOfSrtrk,
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
        _unitOfWork = unitOfSrtrk;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _assignedBarcodeMatchingService = assignedBarcodeMatchingService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SrtImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<SrtImportLineDto>>.SuccessResult(new PagedResponse<SrtImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SrtImportLineNotFound");
            return ApiResponse<SrtImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<SrtImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<SrtImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SrtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtImportLineDto>> CreateAsync(CreateSrtImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SrtImportLine>(createDto) ?? new SrtImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SrtImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SrtImportLineDto>> UpdateAsync(long id, UpdateSrtImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SrtImportLineNotFound");
            return ApiResponse<SrtImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SrtImportLineDto>(entity);
        return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("SrtImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<SrtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSrtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Quantity <= 0)
            {
                return await RollbackImportLineErrorAsync("SrtImportLineQuantityInvalid", 400, cancellationToken);
            }

            var header = await _headers.GetByIdAsync(request.HeaderId, cancellationToken);
            if (header == null || header.IsDeleted)
            {
                return await RollbackImportLineErrorAsync("SrtHeaderNotFound", 404, cancellationToken);
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

            var matchResult = await _assignedBarcodeMatchingService.MatchAsync(new AssignedBarcodeMatchRequest<SrtLine, SrtLineSerial>
            {
                BarcodeRequest = new ResolveBarcodeRequestDto
                {
                    ModuleKey = BarcodeModuleKeys.SubcontractingReceiptAssigned,
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
                StockAndYapKodNotMatchedErrorCode = "SrtImportLineStockAndYapKodNotMatched",
                SerialNotMatchedErrorCode = "SrtImportLineSerialNotMatched",
                NoMatchingLineErrorCode = "SrtImportLineMatchingLineNotFound",
                QuantityExceededErrorCode = "SrtHeaderQuantityCannotBeGreater"
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
                importLine = new SrtImportLine
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

            var route = new SrtRoute
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

            var dto = _mapper.Map<SrtImportLineDto>(importLine);
            await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
            return ApiResponse<SrtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtImportLineCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<SrtImportLineDto>>(importLines);
        var routeDtos = _mapper.Map<List<SrtRouteDto>>(routes);

        var result = lineDtos.Select(importLine => new SrtImportLineWithRoutesDto
        {
            ImportLine = importLine,
            Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtImportLineRetrievedSuccessfully"));
    }

    private async Task<ApiResponse<SrtImportLineDto>> RollbackImportLineErrorAsync(string localizationKey, int statusCode, CancellationToken cancellationToken, object? details = null)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(localizationKey);
        return ApiResponse<SrtImportLineDto>.ErrorResult(message, message, statusCode, errorCode: localizationKey, details: details);
    }
}

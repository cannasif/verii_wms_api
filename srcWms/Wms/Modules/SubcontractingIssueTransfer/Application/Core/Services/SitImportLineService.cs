using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitImportLineService : ISitImportLineService
{
    private readonly IRepository<SitImportLine> _importLines;
    private readonly IRepository<SitHeader> _headers;
    private readonly IRepository<SitLine> _lines;
    private readonly IRepository<SitLineSerial> _lineSerials;
    private readonly IRepository<SitRoute> _routes;
    private readonly IRepository<SitParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IAssignedBarcodeMatchingService _assignedBarcodeMatchingService;
    private readonly IMapper _mapper;

    public SitImportLineService(
        IRepository<SitImportLine> importLines,
        IRepository<SitHeader> headers,
        IRepository<SitLine> lines,
        IRepository<SitLineSerial> lineSerials,
        IRepository<SitRoute> routes,
        IRepository<SitParameter> parameters,
        IUnitOfWork unitOfSitrk,
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
        _unitOfWork = unitOfSitrk;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _assignedBarcodeMatchingService = assignedBarcodeMatchingService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SitImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<SitImportLineDto>>.SuccessResult(new PagedResponse<SitImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SitImportLineNotFound");
            return ApiResponse<SitImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<SitImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<SitImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitImportLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SitImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SitImportLine>(createDto) ?? new SitImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SitImportLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SitImportLineNotFound");
            return ApiResponse<SitImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SitImportLineDto>(entity);
        return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("SitImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<SitImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSitImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Quantity <= 0)
            {
                return await RollbackImportLineErrorAsync("SitImportLineQuantityInvalid", 400, cancellationToken);
            }

            var header = await _headers.GetByIdAsync(request.HeaderId, cancellationToken);
            if (header == null || header.IsDeleted)
            {
                return await RollbackImportLineErrorAsync("SitHeaderNotFound", 404, cancellationToken);
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

            var matchResult = await _assignedBarcodeMatchingService.MatchAsync(new AssignedBarcodeMatchRequest<SitLine, SitLineSerial>
            {
                BarcodeRequest = new ResolveBarcodeRequestDto
                {
                    ModuleKey = BarcodeModuleKeys.SubcontractingIssueAssigned,
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
                StockAndYapKodNotMatchedErrorCode = "SitImportLineStockAndYapKodNotMatched",
                SerialNotMatchedErrorCode = "SitImportLineSerialNotMatched",
                NoMatchingLineErrorCode = "SitImportLineMatchingLineNotFound",
                QuantityExceededErrorCode = "SitHeaderQuantityCannotBeGreater"
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
                importLine = new SitImportLine
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

            var route = new SitRoute
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

            var dto = _mapper.Map<SitImportLineDto>(importLine);
            await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
            return ApiResponse<SitImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitImportLineCreatedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<SitImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<SitImportLineDto>>(importLines);
        var routeDtos = _mapper.Map<List<SitRouteDto>>(routes);

        var result = lineDtos.Select(importLine => new SitImportLineWithRoutesDto
        {
            ImportLine = importLine,
            Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<SitImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitImportLineRetrievedSuccessfully"));
    }

    private async Task<ApiResponse<SitImportLineDto>> RollbackImportLineErrorAsync(string localizationKey, int statusCode, CancellationToken cancellationToken, object? details = null)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        var message = _localizationService.GetLocalizedString(localizationKey);
        return ApiResponse<SitImportLineDto>.ErrorResult(message, message, statusCode, errorCode: localizationKey, details: details);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrImportLineService : IPrImportLineService
{
    private readonly IRepository<PrImportLine> _importLines;
    private readonly IRepository<PrHeader> _headers;
    private readonly IRepository<PrLine> _lines;
    private readonly IRepository<PrRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrImportLineService(
        IRepository<PrImportLine> importLines,
        IRepository<PrHeader> headers,
        IRepository<PrLine> lines,
        IRepository<PrRoute> routes,
        IUnitOfWork unitOfPrrk,
        IErpReadEnrichmentService erpReadEnrichmentService,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _importLines = importLines;
        _headers = headers;
        _lines = lines;
        _routes = routes;
        _unitOfWork = unitOfPrrk;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrImportLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrImportLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<PrImportLineDto>>.SuccessResult(new PagedResponse<PrImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PrImportLineNotFound");
            return ApiResponse<PrImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PrImportLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PrImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrImportLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrImportLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PrImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrImportLineDto>> CreateAsync(CreatePrImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PrImportLine>(createDto) ?? new PrImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PrImportLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrImportLineDto>> UpdateAsync(long id, UpdatePrImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PrImportLineNotFound");
            return ApiResponse<PrImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PrImportLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("PrImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddPrImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            var msg = _localizationService.GetLocalizedString("PrImportLineQuantityInvalid");
            return ApiResponse<PrImportLineDto>.ErrorResult(msg, msg, 400);
        }

        var entity = new PrImportLine
        {
            HeaderId = request.HeaderId,
            LineId = request.LineId,
            StockCode = request.StockCode,
            YapKod = request.YapKod,
            Description = request.StockName ?? request.Barcode,
            Description1 = request.Barcode,
            CreatedDate = DateTimeProvider.Now
        };

        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PrImportLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PrImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<PrImportLineDto>>(importLines);
        lineDtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(lineDtos, cancellationToken)).Data?.ToList() ?? lineDtos;
        var routeDtos = _mapper.Map<List<PrRouteDto>>(routes);

        var result = lineDtos.Select(importLine => new PrImportLineWithRoutesDto
        {
            ImportLine = importLine,
            Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PrImportLineRetrievedSuccessfully"));
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Services;

public sealed class WtImportLineService : IWtImportLineService
{
    private readonly IRepository<WtImportLine> _importLines;
    private readonly IRepository<WtHeader> _headers;
    private readonly IRepository<WtLine> _lines;
    private readonly IRepository<WtRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WtImportLineService(
        IRepository<WtImportLine> importLines,
        IRepository<WtHeader> headers,
        IRepository<WtLine> lines,
        IRepository<WtRoute> routes,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _importLines = importLines;
        _headers = headers;
        _lines = lines;
        _routes = routes;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtImportLineDto>>(items);
        return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtImportLineDto>>(items);
        return ApiResponse<PagedResponse<WtImportLineDto>>.SuccessResult(new PagedResponse<WtImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WtImportLineNotFound");
            return ApiResponse<WtImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WtImportLineDto>(entity);
        return ApiResponse<WtImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtImportLineDto>>(items);
        return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtImportLineDto>>(items);
        return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtImportLineDto>>> GetByRouteIdAsync(long routeId, CancellationToken cancellationToken = default)
    {
        var importLineIds = await _routes.Query()
            .Where(x => x.Id == routeId)
            .Select(x => x.ImportLineId)
            .ToListAsync(cancellationToken);
        var items = await _importLines.Query().Where(x => importLineIds.Contains(x.Id)).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WtImportLineDto>>(items);
        return ApiResponse<IEnumerable<WtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtImportLineDto>> CreateAsync(CreateWtImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WtImportLine>(createDto) ?? new WtImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WtImportLineDto>(entity);
        return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WtImportLineDto>> UpdateAsync(long id, UpdateWtImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WtImportLineNotFound");
            return ApiResponse<WtImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WtImportLineDto>(entity);
        return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("WtImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<WtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddWtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            var msg = _localizationService.GetLocalizedString("WtImportLineQuantityInvalid");
            return ApiResponse<WtImportLineDto>.ErrorResult(msg, msg, 400);
        }

        var entity = new WtImportLine
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
        var dto = _mapper.Map<WtImportLineDto>(entity);
        return ApiResponse<WtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<WtImportLineDto>>(importLines);
        var routeDtos = _mapper.Map<List<WtRouteDto>>(routes);

        var result = lineDtos
            .Select(importLine => new WtImportLineWithRoutesDto
            {
                ImportLine = importLine,
                Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
            })
            .ToList();

        return ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtImportLineRetrievedSuccessfully"));
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtImportLineService : IPtImportLineService
{
    private readonly IRepository<PtImportLine> _importLines;
    private readonly IRepository<PtHeader> _headers;
    private readonly IRepository<PtLine> _lines;
    private readonly IRepository<PtRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PtImportLineService(
        IRepository<PtImportLine> importLines,
        IRepository<PtHeader> headers,
        IRepository<PtLine> lines,
        IRepository<PtRoute> routes,
        IUnitOfWork unitOfPtrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _importLines = importLines;
        _headers = headers;
        _lines = lines;
        _routes = routes;
        _unitOfWork = unitOfPtrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtImportLineDto>>(items);
        return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtImportLineDto>>(items);
        return ApiResponse<PagedResponse<PtImportLineDto>>.SuccessResult(new PagedResponse<PtImportLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PtImportLineNotFound");
            return ApiResponse<PtImportLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PtImportLineDto>(entity);
        return ApiResponse<PtImportLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtImportLineDto>>(items);
        return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _importLines.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtImportLineDto>>(items);
        return ApiResponse<IEnumerable<PtImportLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtImportLineDto>> CreateAsync(CreatePtImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtImportLine>(createDto) ?? new PtImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PtImportLineDto>(entity);
        return ApiResponse<PtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtImportLineDto>> UpdateAsync(long id, UpdatePtImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PtImportLineNotFound");
            return ApiResponse<PtImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PtImportLineDto>(entity);
        return ApiResponse<PtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        var routesExist = await _routes.Query().Where(x => x.ImportLineId == id).AnyAsync(cancellationToken);
        if (routesExist)
        {
            var msg = _localizationService.GetLocalizedString("PtImportLineRoutesExist");
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
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<PtImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddPtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            var msg = _localizationService.GetLocalizedString("PtImportLineQuantityInvalid");
            return ApiResponse<PtImportLineDto>.ErrorResult(msg, msg, 400);
        }

        var entity = new PtImportLine
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
        var dto = _mapper.Map<PtImportLineDto>(entity);
        return ApiResponse<PtImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var importLineIds = importLines.Select(x => x.Id).ToList();
        var routes = await _routes.Query().Where(x => importLineIds.Contains(x.ImportLineId)).ToListAsync(cancellationToken);

        var lineDtos = _mapper.Map<List<PtImportLineDto>>(importLines);
        var routeDtos = _mapper.Map<List<PtRouteDto>>(routes);

        var result = lineDtos.Select(importLine => new PtImportLineWithRoutesDto
        {
            ImportLine = importLine,
            Routes = routeDtos.Where(x => x.ImportLineId == importLine.Id).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<PtImportLineWithRoutesDto>>.SuccessResult(result, _localizationService.GetLocalizedString("PtImportLineRetrievedSuccessfully"));
    }
}

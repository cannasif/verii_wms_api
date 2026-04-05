using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcImportLineService : IIcImportLineService
{
    private readonly IRepository<IcHeader> _headers;
    private readonly IRepository<IcImportLine> _importLines;
    private readonly IRepository<IcRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;

    public IcImportLineService(
        IRepository<IcHeader> headers,
        IRepository<IcImportLine> importLines,
        IRepository<IcRoute> routes,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
    {
        _headers = headers;
        _importLines = importLines;
        _routes = routes;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = _mapper.Map<List<IcImportLineDto>>(await _importLines.Query().ToListAsync(cancellationToken));
        return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(items, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<IcImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _importLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = _mapper.Map<List<IcImportLineDto>>(await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken));

        return ApiResponse<PagedResponse<IcImportLineDto>>.SuccessResult(
            new PagedResponse<IcImportLineDto>(items, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize),
            _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcImportLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcImportLineNotFound");
            return ApiResponse<IcImportLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<IcImportLineDto>(entity);
        return ApiResponse<IcImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = _mapper.Map<List<IcImportLineDto>>(await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken));
        return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(items, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var header = await _headers.GetByIdAsync(headerId, cancellationToken);
        if (header == null || header.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>.ErrorResult(msg, msg, 404);
        }

        var importLines = await _importLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var items = new List<IcImportLineWithRoutesDto>();
        foreach (var il in importLines)
        {
            var routes = await _routes.Query().Where(r => r.ImportLineId == il.Id && !r.IsDeleted).ToListAsync(cancellationToken);
            items.Add(new IcImportLineWithRoutesDto
            {
                ImportLine = _mapper.Map<IcImportLineDto>(il),
                Routes = _mapper.Map<List<IcRouteDto>>(routes),
            });
        }

        return ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcImportLineDto>> CreateAsync(CreateIcImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcImportLine>(createDto) ?? new IcImportLine();
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;

        await _importLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<IcImportLineDto>.SuccessResult(_mapper.Map<IcImportLineDto>(entity), _localizationService.GetLocalizedString("IcImportLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcImportLineDto>> UpdateAsync(long id, UpdateIcImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcImportLineNotFound");
            return ApiResponse<IcImportLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _importLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<IcImportLineDto>.SuccessResult(_mapper.Map<IcImportLineDto>(entity), _localizationService.GetLocalizedString("IcImportLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _importLines.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcImportLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (await _routes.Query().AnyAsync(x => x.ImportLineId == id && !x.IsDeleted, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcImportLineRoutesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _importLines.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcImportLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

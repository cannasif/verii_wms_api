using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrRouteService : IGrRouteService
{
    private readonly IRepository<GrRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public GrRouteService(
        IRepository<GrRoute> routes,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(_mapper.Map<List<GrRouteDto>>(items), _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<GrRouteDto>>.SuccessResult(new PagedResponse<GrRouteDto>(_mapper.Map<List<GrRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrRouteNotFound");
            return ApiResponse<GrRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<GrRouteDto>.SuccessResult(_mapper.Map<GrRouteDto>(entity), _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByImportLineIdAsync(long importLineId, CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().Where(x => x.ImportLineId == importLineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(_mapper.Map<List<GrRouteDto>>(items), _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().Where(x => x.ImportLine.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(_mapper.Map<List<GrRouteDto>>(items), _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrRouteDto>> CreateAsync(CreateGrRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrRoute>(createDto) ?? new GrRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrRouteDto>.SuccessResult(_mapper.Map<GrRouteDto>(entity), _localizationService.GetLocalizedString("GrRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrRouteDto>> UpdateAsync(long id, UpdateGrRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrRouteNotFound");
            return ApiResponse<GrRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrRouteDto>.SuccessResult(_mapper.Map<GrRouteDto>(entity), _localizationService.GetLocalizedString("GrRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("GrRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrRouteDeletedSuccessfully"));
    }
}

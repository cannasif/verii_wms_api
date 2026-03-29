using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Services;

public sealed class ShRouteService : IShRouteService
{
    private readonly IRepository<ShRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public ShRouteService(
        IRepository<ShRoute> routes,
        IUnitOfWork unitOfShrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfShrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(_mapper.Map<List<ShRouteDto>>(items), _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ShRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<ShRouteDto>>.SuccessResult(new PagedResponse<ShRouteDto>(_mapper.Map<List<ShRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShRouteNotFound");
            return ApiResponse<ShRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<ShRouteDto>.SuccessResult(_mapper.Map<ShRouteDto>(entity), _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(_mapper.Map<List<ShRouteDto>>(items), _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ShRoute>(createDto) ?? new ShRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShRouteDto>.SuccessResult(_mapper.Map<ShRouteDto>(entity), _localizationService.GetLocalizedString("ShRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShRouteNotFound");
            return ApiResponse<ShRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShRouteDto>.SuccessResult(_mapper.Map<ShRouteDto>(entity), _localizationService.GetLocalizedString("ShRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("ShRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShRouteDeletedSuccessfully"));
    }
}

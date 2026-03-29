using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoRouteService : IWoRouteService
{
    private readonly IRepository<WoRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WoRouteService(
        IRepository<WoRoute> routes,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(_mapper.Map<List<WoRouteDto>>(items), _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WoRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WoRouteDto>>.SuccessResult(new PagedResponse<WoRouteDto>(_mapper.Map<List<WoRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoRouteNotFound");
            return ApiResponse<WoRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WoRouteDto>.SuccessResult(_mapper.Map<WoRouteDto>(entity), _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(_mapper.Map<List<WoRouteDto>>(items), _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoRouteDto>> CreateAsync(CreateWoRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WoRoute>(createDto) ?? new WoRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoRouteDto>.SuccessResult(_mapper.Map<WoRouteDto>(entity), _localizationService.GetLocalizedString("WoRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WoRouteDto>> UpdateAsync(long id, UpdateWoRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoRouteNotFound");
            return ApiResponse<WoRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoRouteDto>.SuccessResult(_mapper.Map<WoRouteDto>(entity), _localizationService.GetLocalizedString("WoRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WoRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoRouteDeletedSuccessfully"));
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Services;

public sealed class WtRouteService : IWtRouteService
{
    private readonly IRepository<WtRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WtRouteService(
        IRepository<WtRoute> routes,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(_mapper.Map<List<WtRouteDto>>(items), _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WtRouteDto>>.SuccessResult(new PagedResponse<WtRouteDto>(_mapper.Map<List<WtRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtRouteNotFound");
            return ApiResponse<WtRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WtRouteDto>.SuccessResult(_mapper.Map<WtRouteDto>(entity), _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query()
            .Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial)
            .ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(_mapper.Map<List<WtRouteDto>>(items), _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WtRoute>(createDto) ?? new WtRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtRouteDto>.SuccessResult(_mapper.Map<WtRouteDto>(entity), _localizationService.GetLocalizedString("WtRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtRouteNotFound");
            return ApiResponse<WtRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtRouteDto>.SuccessResult(_mapper.Map<WtRouteDto>(entity), _localizationService.GetLocalizedString("WtRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WtRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtRouteDeletedSuccessfully"));
    }
}

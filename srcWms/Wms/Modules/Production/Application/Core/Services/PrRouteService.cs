using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrRouteService : IPrRouteService
{
    private readonly IRepository<PrRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrRouteService(
        IRepository<PrRoute> routes,
        IUnitOfWork unitOfPrrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfPrrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(_mapper.Map<List<PrRouteDto>>(items), _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PrRouteDto>>.SuccessResult(new PagedResponse<PrRouteDto>(_mapper.Map<List<PrRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrRouteNotFound");
            return ApiResponse<PrRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PrRouteDto>.SuccessResult(_mapper.Map<PrRouteDto>(entity), _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(_mapper.Map<List<PrRouteDto>>(items), _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrRouteDto>> CreateAsync(CreatePrRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PrRoute>(createDto) ?? new PrRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrRouteDto>.SuccessResult(_mapper.Map<PrRouteDto>(entity), _localizationService.GetLocalizedString("PrRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrRouteDto>> UpdateAsync(long id, UpdatePrRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrRouteNotFound");
            return ApiResponse<PrRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrRouteDto>.SuccessResult(_mapper.Map<PrRouteDto>(entity), _localizationService.GetLocalizedString("PrRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PrRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrRouteDeletedSuccessfully"));
    }
}

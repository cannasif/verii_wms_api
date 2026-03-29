using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtRouteService : IPtRouteService
{
    private readonly IRepository<PtRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PtRouteService(
        IRepository<PtRoute> routes,
        IUnitOfWork unitOfPtrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfPtrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(_mapper.Map<List<PtRouteDto>>(items), _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PtRouteDto>>.SuccessResult(new PagedResponse<PtRouteDto>(_mapper.Map<List<PtRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtRouteNotFound");
            return ApiResponse<PtRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PtRouteDto>.SuccessResult(_mapper.Map<PtRouteDto>(entity), _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtRouteDto>>.SuccessResult(_mapper.Map<List<PtRouteDto>>(items), _localizationService.GetLocalizedString("PtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtRouteDto>> CreateAsync(CreatePtRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtRoute>(createDto) ?? new PtRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtRouteDto>.SuccessResult(_mapper.Map<PtRouteDto>(entity), _localizationService.GetLocalizedString("PtRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtRouteDto>> UpdateAsync(long id, UpdatePtRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtRouteNotFound");
            return ApiResponse<PtRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtRouteDto>.SuccessResult(_mapper.Map<PtRouteDto>(entity), _localizationService.GetLocalizedString("PtRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PtRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtRouteDeletedSuccessfully"));
    }
}

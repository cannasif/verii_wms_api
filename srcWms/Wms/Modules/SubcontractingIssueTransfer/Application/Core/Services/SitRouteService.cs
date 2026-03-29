using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitRouteService : ISitRouteService
{
    private readonly IRepository<SitRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SitRouteService(
        IRepository<SitRoute> routes,
        IUnitOfWork unitOfSitrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfSitrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(_mapper.Map<List<SitRouteDto>>(items), _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SitRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SitRouteDto>>.SuccessResult(new PagedResponse<SitRouteDto>(_mapper.Map<List<SitRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitRouteNotFound");
            return ApiResponse<SitRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SitRouteDto>.SuccessResult(_mapper.Map<SitRouteDto>(entity), _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitRouteDto>>.SuccessResult(_mapper.Map<List<SitRouteDto>>(items), _localizationService.GetLocalizedString("SitRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitRouteDto>> CreateAsync(CreateSitRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SitRoute>(createDto) ?? new SitRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitRouteDto>.SuccessResult(_mapper.Map<SitRouteDto>(entity), _localizationService.GetLocalizedString("SitRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SitRouteDto>> UpdateAsync(long id, UpdateSitRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitRouteNotFound");
            return ApiResponse<SitRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitRouteDto>.SuccessResult(_mapper.Map<SitRouteDto>(entity), _localizationService.GetLocalizedString("SitRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SitRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitRouteDeletedSuccessfully"));
    }
}

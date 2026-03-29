using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtRouteService : ISrtRouteService
{
    private readonly IRepository<SrtRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SrtRouteService(
        IRepository<SrtRoute> routes,
        IUnitOfWork unitOfSrtrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfSrtrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(_mapper.Map<List<SrtRouteDto>>(items), _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SrtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SrtRouteDto>>.SuccessResult(new PagedResponse<SrtRouteDto>(_mapper.Map<List<SrtRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtRouteNotFound");
            return ApiResponse<SrtRouteDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SrtRouteDto>.SuccessResult(_mapper.Map<SrtRouteDto>(entity), _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
    {
        var normalizedSerial = (serialNo ?? string.Empty).Trim();
        var items = await _routes.Query().Where(x => (x.SerialNo ?? string.Empty).Trim() == normalizedSerial).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(_mapper.Map<List<SrtRouteDto>>(items), _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtRouteDto>> CreateAsync(CreateSrtRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SrtRoute>(createDto) ?? new SrtRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtRouteDto>.SuccessResult(_mapper.Map<SrtRouteDto>(entity), _localizationService.GetLocalizedString("SrtRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SrtRouteDto>> UpdateAsync(long id, UpdateSrtRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtRouteNotFound");
            return ApiResponse<SrtRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtRouteDto>.SuccessResult(_mapper.Map<SrtRouteDto>(entity), _localizationService.GetLocalizedString("SrtRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SrtRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtRouteDeletedSuccessfully"));
    }
}

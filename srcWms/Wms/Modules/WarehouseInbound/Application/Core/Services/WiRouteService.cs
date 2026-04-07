using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Services;

public sealed class WiRouteService : IWiRouteService
{
    private readonly IRepository<WiRoute> _routes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public WiRouteService(
        IRepository<WiRoute> routes,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IMapper mapper)
    {
        _routes = routes;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WiRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _routes.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WiRouteDto>>(items);
        await _documentReferenceReadEnricher.EnrichRoutesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<WiRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WiRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WiRouteDto>>(items);
        await _documentReferenceReadEnricher.EnrichRoutesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<WiRouteDto>>.SuccessResult(new PagedResponse<WiRouteDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WiRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiRouteNotFound");
            return ApiResponse<WiRouteDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WiRouteDto>(entity);
        await _documentReferenceReadEnricher.EnrichRoutesAsync(new List<WiRouteDto> { dto }, cancellationToken);
        return ApiResponse<WiRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiRouteDto>> CreateAsync(CreateWiRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WiRoute>(createDto) ?? new WiRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiRouteDto>.SuccessResult(_mapper.Map<WiRouteDto>(entity), _localizationService.GetLocalizedString("WiRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WiRouteDto>> UpdateAsync(long id, UpdateWiRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiRouteNotFound");
            return ApiResponse<WiRouteDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiRouteDto>.SuccessResult(_mapper.Map<WiRouteDto>(entity), _localizationService.GetLocalizedString("WiRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _routes.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WiRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _routes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiRouteDeletedSuccessfully"));
    }
}

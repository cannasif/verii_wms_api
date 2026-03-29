using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcRouteService : IIcRouteService
{
    private readonly IRepository<IcRoute> _routes;
    private readonly IRepository<IcImportLine> _importLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;

    public IcRouteService(IRepository<IcRoute> routes, IRepository<IcImportLine> importLines, IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
    {
        _routes = routes;
        _importLines = importLines;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(_mapper.Map<List<IcRouteDto>>(await _routes.Query().ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));

    public async Task<ApiResponse<PagedResponse<IcRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _routes.Query().ApplyFilters(request.Filters, request.FilterLogic).ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<IcRouteDto>>.SuccessResult(new PagedResponse<IcRouteDto>(_mapper.Map<List<IcRouteDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcRouteNotFound");
            return ApiResponse<IcRouteDto>.ErrorResult(msg, msg, 404);
        }
        return ApiResponse<IcRouteDto>.SuccessResult(_mapper.Map<IcRouteDto>(entity), _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetByImportLineIdAsync(long importLineId, CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(_mapper.Map<List<IcRouteDto>>(await _routes.Query().Where(x => x.ImportLineId == importLineId).ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));

    public async Task<ApiResponse<IcRouteDto>> CreateAsync(CreateIcRouteDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcRoute>(createDto) ?? new IcRoute();
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        await _routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcRouteDto>.SuccessResult(_mapper.Map<IcRouteDto>(entity), _localizationService.GetLocalizedString("IcRouteCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcRouteDto>> UpdateAsync(long id, UpdateIcRouteDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _routes.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcRouteNotFound");
            return ApiResponse<IcRouteDto>.ErrorResult(msg, msg, 404);
        }
        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _routes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcRouteDto>.SuccessResult(_mapper.Map<IcRouteDto>(entity), _localizationService.GetLocalizedString("IcRouteUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var route = await _routes.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (route == null || route.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcRouteNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }
        var remainingRoutesCount = await _routes.Query().Where(r => !r.IsDeleted && r.ImportLineId == route.ImportLineId && r.Id != id).CountAsync(cancellationToken);
        var shouldDeleteImportLine = remainingRoutesCount == 0;
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _routes.SoftDelete(id, cancellationToken);
            if (shouldDeleteImportLine)
            {
                var importLine = await _importLines.GetByIdAsync(route.ImportLineId, cancellationToken);
                if (importLine != null && !importLine.IsDeleted)
                {
                    await _importLines.SoftDelete(route.ImportLineId, cancellationToken);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcRouteDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

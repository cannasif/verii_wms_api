using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtLineService : IPtLineService
{
    private readonly IRepository<PtLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PtLineService(
        IRepository<PtLine> lines,
        IUnitOfWork unitOfPtrk,
        IErpReadEnrichmentService erpReadEnrichmentService,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfPtrk;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<PtLineDto>>.SuccessResult(new PagedResponse<PtLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtLineNotFound");
            return ApiResponse<PtLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<PtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineDto>> CreateAsync(CreatePtLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtLine>(createDto) ?? new PtLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineDto>> UpdateAsync(long id, UpdatePtLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtLineNotFound");
            return ApiResponse<PtLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<PtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PtLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtLineDeletedSuccessfully"));
    }
}

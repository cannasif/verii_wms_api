using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Services;

public sealed class WiLineService : IWiLineService
{
    private readonly IRepository<WiLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WiLineService(
        IRepository<WiLine> lines,
        IUnitOfWork unitOfWork,
        IErpReadEnrichmentService erpReadEnrichmentService,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfWork;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WiLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WiLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WiLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<WiLineDto>>.SuccessResult(new PagedResponse<WiLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiLineNotFound");
            return ApiResponse<WiLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WiLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WiLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineDto>> CreateAsync(CreateWiLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WiLine>(createDto) ?? new WiLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WiLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineDto>> UpdateAsync(long id, UpdateWiLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiLineNotFound");
            return ApiResponse<WiLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WiLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WiLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiLineDeletedSuccessfully"));
    }
}

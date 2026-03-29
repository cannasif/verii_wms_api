using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitLineService : ISitLineService
{
    private readonly IRepository<SitLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SitLineService(
        IRepository<SitLine> lines,
        IUnitOfWork unitOfSitrk,
        IErpReadEnrichmentService erpReadEnrichmentService,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfSitrk;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SitLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<SitLineDto>>.SuccessResult(new PagedResponse<SitLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitLineNotFound");
            return ApiResponse<SitLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<SitLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SitLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineDto>> CreateAsync(CreateSitLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SitLine>(createDto) ?? new SitLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SitLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineDto>> UpdateAsync(long id, UpdateSitLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitLineNotFound");
            return ApiResponse<SitLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SitLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SitLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitLineDeletedSuccessfully"));
    }
}

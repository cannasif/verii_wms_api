using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtLineService : ISrtLineService
{
    private readonly IRepository<SrtLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IErpReadEnrichmentService _erpReadEnrichmentService;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SrtLineService(
        IRepository<SrtLine> lines,
        IUnitOfWork unitOfSrtrk,
        IErpReadEnrichmentService erpReadEnrichmentService,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfSrtrk;
        _erpReadEnrichmentService = erpReadEnrichmentService;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<PagedResponse<SrtLineDto>>.SuccessResult(new PagedResponse<SrtLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineNotFound");
            return ApiResponse<SrtLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<SrtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtLineDto>>(items);
        dtos = (await _erpReadEnrichmentService.PopulateStockNamesAsync(dtos, cancellationToken)).Data?.ToList() ?? dtos;
        return ApiResponse<IEnumerable<SrtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SrtLine>(createDto) ?? new SrtLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SrtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineNotFound");
            return ApiResponse<SrtLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<SrtLineDto>(entity);
        dto = (await _erpReadEnrichmentService.PopulateStockNamesAsync(new[] { dto }, cancellationToken)).Data?.FirstOrDefault() ?? dto;
        return ApiResponse<SrtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtLineDeletedSuccessfully"));
    }
}

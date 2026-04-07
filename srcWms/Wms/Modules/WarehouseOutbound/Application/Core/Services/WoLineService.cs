using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoLineService : IWoLineService
{
    private readonly IRepository<WoLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public WoLineService(
        IRepository<WoLine> lines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WoLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<WoLineDto>>.SuccessResult(new PagedResponse<WoLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoLineNotFound");
            return ApiResponse<WoLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<WoLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineDto>> CreateAsync(CreateWoLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WoLine>(createDto) ?? new WoLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WoLineDto>(entity);
        return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineDto>> UpdateAsync(long id, UpdateWoLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoLineNotFound");
            return ApiResponse<WoLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<WoLineDto>(entity);
        return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WoLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoLineDeletedSuccessfully"));
    }
}

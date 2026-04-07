using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrLineService : IGrLineService
{
    private readonly IRepository<GrLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public GrLineService(
        IRepository<GrLine> lines,
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

    public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<GrLineDto>>.SuccessResult(new PagedResponse<GrLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrLineNotFound");
            return ApiResponse<GrLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<GrLineDto>(entity);
        await _documentReferenceReadEnricher.EnrichLinesAsync(new List<object> { dto }, cancellationToken);
        return ApiResponse<GrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineDto>>(items);
        await _documentReferenceReadEnricher.EnrichLinesAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineDto>> CreateAsync(CreateGrLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrLine>(createDto) ?? new GrLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GrLineDto>(entity);
        return ApiResponse<GrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineDto>> UpdateAsync(long id, UpdateGrLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrLineNotFound");
            return ApiResponse<GrLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GrLineDto>(entity);
        return ApiResponse<GrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("GrLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrLineDeletedSuccessfully"));
    }
}

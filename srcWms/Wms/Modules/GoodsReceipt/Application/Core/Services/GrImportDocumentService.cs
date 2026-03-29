using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrImportDocumentService : IGrImportDocumentService
{
    private readonly IRepository<GrImportDocument> _documents;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public GrImportDocumentService(
        IRepository<GrImportDocument> documents,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _documents = documents;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _documents.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(_mapper.Map<List<GrImportDocumentDto>>(items), _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrImportDocumentDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _documents.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<GrImportDocumentDto>>.SuccessResult(new PagedResponse<GrImportDocumentDto>(_mapper.Map<List<GrImportDocumentDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportDocumentDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _documents.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrImportDocumentNotFound");
            return ApiResponse<GrImportDocumentDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<GrImportDocumentDto>.SuccessResult(_mapper.Map<GrImportDocumentDto>(entity), _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _documents.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(_mapper.Map<List<GrImportDocumentDto>>(items), _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportDocumentDto>> CreateAsync(CreateGrImportDocumentDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrImportDocument>(createDto) ?? new GrImportDocument();
        entity.CreatedDate = DateTimeProvider.Now;
        await _documents.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrImportDocumentDto>.SuccessResult(_mapper.Map<GrImportDocumentDto>(entity), _localizationService.GetLocalizedString("GrImportDocumentCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrImportDocumentDto>> UpdateAsync(long id, UpdateGrImportDocumentDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _documents.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrImportDocumentNotFound");
            return ApiResponse<GrImportDocumentDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _documents.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrImportDocumentDto>.SuccessResult(_mapper.Map<GrImportDocumentDto>(entity), _localizationService.GetLocalizedString("GrImportDocumentUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _documents.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("GrImportDocumentNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _documents.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportDocumentDeletedSuccessfully"));
    }
}

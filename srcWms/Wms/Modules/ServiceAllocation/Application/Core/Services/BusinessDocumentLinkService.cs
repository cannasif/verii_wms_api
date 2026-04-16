using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Common;
using BusinessDocumentLinkEntity = Wms.Domain.Entities.ServiceAllocation.BusinessDocumentLink;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class BusinessDocumentLinkService : IBusinessDocumentLinkService
{
    private readonly IRepository<BusinessDocumentLinkEntity> _documentLinks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public BusinessDocumentLinkService(
        IRepository<BusinessDocumentLinkEntity> documentLinks,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _documentLinks = documentLinks;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await BuildQuery().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<BusinessDocumentLinkDto>>.SuccessResult(_mapper.Map<List<BusinessDocumentLinkDto>>(items), _localizationService.GetLocalizedString("BusinessDocumentLinkRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>> GetByBusinessEntityAsync(long businessEntityId, Wms.Domain.Entities.ServiceAllocation.Enums.BusinessEntityType businessEntityType, CancellationToken cancellationToken = default)
    {
        var items = await BuildQuery()
            .Where(x => x.BusinessEntityId == businessEntityId && x.BusinessEntityType == businessEntityType)
            .OrderBy(x => x.SequenceNo)
            .ThenBy(x => x.LinkedAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return ApiResponse<IEnumerable<BusinessDocumentLinkDto>>.SuccessResult(_mapper.Map<List<BusinessDocumentLinkDto>>(items), _localizationService.GetLocalizedString("BusinessDocumentLinkRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<BusinessDocumentLinkDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = BuildQuery()
            .ApplySearch(request.Search, nameof(BusinessDocumentLinkEntity.Note))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(BusinessDocumentLinkEntity.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtoItems = _mapper.Map<List<BusinessDocumentLinkDto>>(items);
        var page = new PagedResponse<BusinessDocumentLinkDto>(dtoItems, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<BusinessDocumentLinkDto>>.SuccessResult(page, _localizationService.GetLocalizedString("BusinessDocumentLinkRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<BusinessDocumentLinkDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _documentLinks.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("BusinessDocumentLinkNotFound");
            return ApiResponse<BusinessDocumentLinkDto>.ErrorResult(message, message, 404);
        }

        return ApiResponse<BusinessDocumentLinkDto>.SuccessResult(_mapper.Map<BusinessDocumentLinkDto>(entity), _localizationService.GetLocalizedString("BusinessDocumentLinkRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<BusinessDocumentLinkDto>> CreateAsync(CreateBusinessDocumentLinkDto createDto, CancellationToken cancellationToken = default)
    {
        var duplicate = await _documentLinks.Query().AnyAsync(
            x => x.BusinessEntityType == createDto.BusinessEntityType
                && x.BusinessEntityId == createDto.BusinessEntityId
                && x.DocumentModule == createDto.DocumentModule
                && x.DocumentHeaderId == createDto.DocumentHeaderId
                && x.DocumentLineId == createDto.DocumentLineId
                && !x.IsDeleted,
            cancellationToken);
        if (duplicate)
        {
            var message = _localizationService.GetLocalizedString("BusinessDocumentLinkAlreadyExists");
            return ApiResponse<BusinessDocumentLinkDto>.ErrorResult(message, message, 400);
        }

        var entity = _mapper.Map<BusinessDocumentLinkEntity>(createDto) ?? new BusinessDocumentLinkEntity();
        Normalize(entity, createDto.BranchCode);
        entity.LinkedAt = createDto.LinkedAt ?? DateTimeProvider.Now;
        entity.CreatedDate = DateTimeProvider.Now;
        await _documentLinks.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<BusinessDocumentLinkDto>.SuccessResult(_mapper.Map<BusinessDocumentLinkDto>(entity), _localizationService.GetLocalizedString("BusinessDocumentLinkCreatedSuccessfully"));
    }

    public async Task<ApiResponse<BusinessDocumentLinkDto>> UpdateAsync(long id, UpdateBusinessDocumentLinkDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _documentLinks.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("BusinessDocumentLinkNotFound");
            return ApiResponse<BusinessDocumentLinkDto>.ErrorResult(message, message, 404);
        }

        _mapper.Map(updateDto, entity);
        Normalize(entity, updateDto.BranchCode);
        if (updateDto.LinkedAt.HasValue)
        {
            entity.LinkedAt = updateDto.LinkedAt.Value;
        }
        entity.UpdatedDate = DateTimeProvider.Now;
        _documentLinks.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<BusinessDocumentLinkDto>.SuccessResult(_mapper.Map<BusinessDocumentLinkDto>(entity), _localizationService.GetLocalizedString("BusinessDocumentLinkUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _documentLinks.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var message = _localizationService.GetLocalizedString("BusinessDocumentLinkNotFound");
            return ApiResponse<bool>.ErrorResult(message, message, 404);
        }

        await _documentLinks.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("BusinessDocumentLinkDeletedSuccessfully"));
    }

    private IQueryable<BusinessDocumentLinkEntity> BuildQuery() => _documentLinks.Query().Where(x => !x.IsDeleted);

    private static void Normalize(BusinessDocumentLinkEntity entity, string? branchCode)
    {
        entity.Note = string.IsNullOrWhiteSpace(entity.Note) ? null : entity.Note.Trim();
        entity.BranchCode = NormalizeBranchCode(branchCode ?? entity.BranchCode);
    }

    private static string NormalizeBranchCode(string? branchCode) => string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();
}

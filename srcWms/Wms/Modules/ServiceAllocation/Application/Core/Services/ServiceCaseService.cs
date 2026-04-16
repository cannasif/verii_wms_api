using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;
using BusinessDocumentLinkEntity = Wms.Domain.Entities.ServiceAllocation.BusinessDocumentLink;
using ServiceCaseEntity = Wms.Domain.Entities.ServiceAllocation.ServiceCase;
using ServiceCaseLineEntity = Wms.Domain.Entities.ServiceAllocation.ServiceCaseLine;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class ServiceCaseService : IServiceCaseService
{
    private readonly IRepository<ServiceCaseEntity> _serviceCases;
    private readonly IRepository<ServiceCaseLineEntity> _serviceCaseLines;
    private readonly IRepository<BusinessDocumentLinkEntity> _documentLinks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public ServiceCaseService(
        IRepository<ServiceCaseEntity> serviceCases,
        IRepository<ServiceCaseLineEntity> serviceCaseLines,
        IRepository<BusinessDocumentLinkEntity> documentLinks,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _serviceCases = serviceCases;
        _serviceCaseLines = serviceCaseLines;
        _documentLinks = documentLinks;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<ServiceCaseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await BuildQuery().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ServiceCaseDto>>.SuccessResult(_mapper.Map<List<ServiceCaseDto>>(items), _localizationService.GetLocalizedString("ServiceCaseRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ServiceCaseDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = BuildQuery()
            .ApplySearch(request.Search,
                nameof(ServiceCaseEntity.CaseNo),
                nameof(ServiceCaseEntity.CustomerCode),
                nameof(ServiceCaseEntity.IncomingStockCode),
                nameof(ServiceCaseEntity.IncomingSerialNo),
                nameof(ServiceCaseEntity.DiagnosisNote))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(ServiceCaseEntity.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtoItems = _mapper.Map<List<ServiceCaseDto>>(items);
        var page = new PagedResponse<ServiceCaseDto>(dtoItems, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<ServiceCaseDto>>.SuccessResult(page, _localizationService.GetLocalizedString("ServiceCaseRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCases.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseNotFound");
            return ApiResponse<ServiceCaseDto>.ErrorResult(message, message, 404);
        }

        return ApiResponse<ServiceCaseDto>.SuccessResult(_mapper.Map<ServiceCaseDto>(entity), _localizationService.GetLocalizedString("ServiceCaseRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseTimelineDto>> GetTimelineAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCases.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseNotFound");
            return ApiResponse<ServiceCaseTimelineDto>.ErrorResult(message, message, 404);
        }

        var lines = await _serviceCaseLines.Query()
            .Where(x => !x.IsDeleted && x.ServiceCaseId == id)
            .OrderBy(x => x.CreatedDate ?? DateTime.MinValue)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var links = await _documentLinks.Query()
            .Where(x => !x.IsDeleted
                && x.BusinessEntityType == BusinessEntityType.ServiceCase
                && x.BusinessEntityId == id
                && x.ServiceCaseId == id)
            .OrderBy(x => x.SequenceNo)
            .ThenBy(x => x.LinkedAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var timeline = new ServiceCaseTimelineDto
        {
            ServiceCase = _mapper.Map<ServiceCaseDto>(entity),
            Lines = _mapper.Map<List<ServiceCaseLineDto>>(lines),
            Timeline = links.Select(x => new ServiceCaseTimelineEventDto
            {
                DocumentLinkId = x.Id,
                DocumentModule = x.DocumentModule,
                DocumentHeaderId = x.DocumentHeaderId,
                DocumentLineId = x.DocumentLineId,
                LinkPurpose = x.LinkPurpose,
                SequenceNo = x.SequenceNo,
                FromWarehouseId = x.FromWarehouseId,
                ToWarehouseId = x.ToWarehouseId,
                Note = x.Note,
                LinkedAt = x.LinkedAt
            }).ToList()
        };

        return ApiResponse<ServiceCaseTimelineDto>.SuccessResult(timeline, _localizationService.GetLocalizedString("ServiceCaseTimelineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseDto>> CreateAsync(CreateServiceCaseDto createDto, CancellationToken cancellationToken = default)
    {
        var normalizedCaseNo = NormalizeCaseNo(createDto.CaseNo);
        var exists = await _serviceCases.Query().AnyAsync(x => x.CaseNo == normalizedCaseNo && !x.IsDeleted, cancellationToken);
        if (exists)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseAlreadyExists");
            return ApiResponse<ServiceCaseDto>.ErrorResult(message, message, 400);
        }

        var entity = _mapper.Map<ServiceCaseEntity>(createDto) ?? new ServiceCaseEntity();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CaseNo = normalizedCaseNo;
        entity.CustomerCode = NormalizeCode(createDto.CustomerCode);
        entity.IncomingStockCode = NormalizeOptionalCode(createDto.IncomingStockCode);
        entity.BranchCode = NormalizeBranchCode(createDto.BranchCode);
        entity.CreatedDate = DateTimeProvider.Now;
        await _serviceCases.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ServiceCaseDto>.SuccessResult(_mapper.Map<ServiceCaseDto>(entity), _localizationService.GetLocalizedString("ServiceCaseCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseDto>> UpdateAsync(long id, UpdateServiceCaseDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCases.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseNotFound");
            return ApiResponse<ServiceCaseDto>.ErrorResult(message, message, 404);
        }

        if (!string.IsNullOrWhiteSpace(updateDto.CaseNo))
        {
            var normalizedCaseNo = NormalizeCaseNo(updateDto.CaseNo);
            var duplicate = await _serviceCases.Query().AnyAsync(x => x.Id != id && x.CaseNo == normalizedCaseNo && !x.IsDeleted, cancellationToken);
            if (duplicate)
            {
                var message = _localizationService.GetLocalizedString("ServiceCaseAlreadyExists");
                return ApiResponse<ServiceCaseDto>.ErrorResult(message, message, 400);
            }
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        if (!string.IsNullOrWhiteSpace(updateDto.CaseNo))
        {
            entity.CaseNo = NormalizeCaseNo(updateDto.CaseNo);
        }
        if (!string.IsNullOrWhiteSpace(updateDto.CustomerCode))
        {
            entity.CustomerCode = NormalizeCode(updateDto.CustomerCode);
        }
        if (!string.IsNullOrWhiteSpace(updateDto.IncomingStockCode))
        {
            entity.IncomingStockCode = NormalizeCode(updateDto.IncomingStockCode);
        }
        if (!string.IsNullOrWhiteSpace(updateDto.BranchCode))
        {
            entity.BranchCode = NormalizeBranchCode(updateDto.BranchCode);
        }

        entity.UpdatedDate = DateTimeProvider.Now;
        _serviceCases.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ServiceCaseDto>.SuccessResult(_mapper.Map<ServiceCaseDto>(entity), _localizationService.GetLocalizedString("ServiceCaseUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _serviceCases.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseNotFound");
            return ApiResponse<bool>.ErrorResult(message, message, 404);
        }

        await _serviceCases.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ServiceCaseDeletedSuccessfully"));
    }

    private IQueryable<ServiceCaseEntity> BuildQuery() => _serviceCases.Query().Where(x => !x.IsDeleted);
    private static string NormalizeCaseNo(string value) => value.Trim().ToUpperInvariant();
    private static string NormalizeCode(string value) => value.Trim().ToUpperInvariant();
    private static string? NormalizeOptionalCode(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    private static string NormalizeBranchCode(string? branchCode) => string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcHeaderService : IIcHeaderService
{
    private readonly IRepository<IcHeader> _headers;
    private readonly IRepository<IcImportLine> _importLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public IcHeaderService(
        IRepository<IcHeader> headers,
        IRepository<IcImportLine> importLines,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _headers = headers;
        _importLines = importLines;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcHeaderDto>>.SuccessResult(_mapper.Map<List<IcHeaderDto>>(await _headers.Query().ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));

    public async Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query().ApplyFilters(request.Filters, request.FilterLogic).ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<IcHeaderDto>>.SuccessResult(new PagedResponse<IcHeaderDto>(_mapper.Map<List<IcHeaderDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<IcHeaderDto>.ErrorResult(msg, msg, 404);
        }
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcHeader>(createDto) ?? new IcHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<IcHeaderDto>.ErrorResult(msg, msg, 404);
        }
        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        if (!await _headers.ExistsAsync(id, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }
        if (await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }
        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcHeaderDeletedSuccessfully"));
    }
}

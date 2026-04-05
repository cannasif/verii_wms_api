using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrLineService : IPrLineService
{
    private readonly IRepository<PrLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrLineService(
        IRepository<PrLine> lines,
        IUnitOfWork unitOfPrrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfPrrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrLineDto>>(items);
        return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrLineDto>>(items);
        return ApiResponse<PagedResponse<PrLineDto>>.SuccessResult(new PagedResponse<PrLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrLineNotFound");
            return ApiResponse<PrLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PrLineDto>(entity);
        return ApiResponse<PrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PrLineDto>>(items);
        return ApiResponse<IEnumerable<PrLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineDto>> CreateAsync(CreatePrLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PrLine>(createDto) ?? new PrLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PrLineDto>(entity);
        return ApiResponse<PrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineDto>> UpdateAsync(long id, UpdatePrLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrLineNotFound");
            return ApiResponse<PrLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<PrLineDto>(entity);
        return ApiResponse<PrLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PrLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrLineDeletedSuccessfully"));
    }
}

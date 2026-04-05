using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Services;

public sealed class ShLineService : IShLineService
{
    private readonly IRepository<ShLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public ShLineService(
        IRepository<ShLine> lines,
        IUnitOfWork unitOfShrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _lines = lines;
        _unitOfWork = unitOfShrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShLineDto>>(items);
        return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ShLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShLineDto>>(items);
        return ApiResponse<PagedResponse<ShLineDto>>.SuccessResult(new PagedResponse<ShLineDto>(dtos, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShLineNotFound");
            return ApiResponse<ShLineDto>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<ShLineDto>(entity);
        return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<ShLineDto>>(items);
        return ApiResponse<IEnumerable<ShLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineDto>> CreateAsync(CreateShLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ShLine>(createDto) ?? new ShLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<ShLineDto>(entity);
        return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineDto>> UpdateAsync(long id, UpdateShLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShLineNotFound");
            return ApiResponse<ShLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<ShLineDto>(entity);
        return ApiResponse<ShLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _lines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("ShLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShLineDeletedSuccessfully"));
    }
}

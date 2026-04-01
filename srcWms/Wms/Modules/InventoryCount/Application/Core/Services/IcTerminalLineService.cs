using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcTerminalLineService : IIcTerminalLineService
{
    private readonly IRepository<IcTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;

    public IcTerminalLineService(IRepository<IcTerminalLine> terminalLines, IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcTerminalLineDto>>.SuccessResult(_mapper.Map<List<IcTerminalLineDto>>(await _terminalLines.Query().ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));

    public async Task<ApiResponse<PagedResponse<IcTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query().ApplyFilters(request.Filters, request.FilterLogic).ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<IcTerminalLineDto>>.SuccessResult(new PagedResponse<IcTerminalLineDto>(_mapper.Map<List<IcTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcTerminalLineNotFound");
            return ApiResponse<IcTerminalLineDto>.ErrorResult(msg, msg, 404);
        }
        return ApiResponse<IcTerminalLineDto>.SuccessResult(_mapper.Map<IcTerminalLineDto>(entity), _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcTerminalLineDto>>.SuccessResult(_mapper.Map<List<IcTerminalLineDto>>(await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));

    public async Task<ApiResponse<IcTerminalLineDto>> CreateAsync(CreateIcTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcTerminalLine>(createDto) ?? new IcTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcTerminalLineDto>.SuccessResult(_mapper.Map<IcTerminalLineDto>(entity), _localizationService.GetLocalizedString("IcTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcTerminalLineDto>> UpdateAsync(long id, UpdateIcTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcTerminalLineNotFound");
            return ApiResponse<IcTerminalLineDto>.ErrorResult(msg, msg, 404);
        }
        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcTerminalLineDto>.SuccessResult(_mapper.Map<IcTerminalLineDto>(entity), _localizationService.GetLocalizedString("IcTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("IcTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcTerminalLineDeletedSuccessfully"));
    }
}

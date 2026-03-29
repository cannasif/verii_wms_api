using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoTerminalLineService : IWoTerminalLineService
{
    private readonly IRepository<WoTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WoTerminalLineService(
        IRepository<WoTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(_mapper.Map<List<WoTerminalLineDto>>(items), _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WoTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WoTerminalLineDto>>.SuccessResult(new PagedResponse<WoTerminalLineDto>(_mapper.Map<List<WoTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoTerminalLineNotFound");
            return ApiResponse<WoTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WoTerminalLineDto>.SuccessResult(_mapper.Map<WoTerminalLineDto>(entity), _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(_mapper.Map<List<WoTerminalLineDto>>(items), _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(_mapper.Map<List<WoTerminalLineDto>>(items), _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoTerminalLineDto>> CreateAsync(CreateWoTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WoTerminalLine>(createDto) ?? new WoTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoTerminalLineDto>.SuccessResult(_mapper.Map<WoTerminalLineDto>(entity), _localizationService.GetLocalizedString("WoTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WoTerminalLineDto>> UpdateAsync(long id, UpdateWoTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WoTerminalLineNotFound");
            return ApiResponse<WoTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoTerminalLineDto>.SuccessResult(_mapper.Map<WoTerminalLineDto>(entity), _localizationService.GetLocalizedString("WoTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WoTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoTerminalLineDeletedSuccessfully"));
    }
}

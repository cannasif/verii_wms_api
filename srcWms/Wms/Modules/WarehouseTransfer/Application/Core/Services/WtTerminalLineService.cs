using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Services;

public sealed class WtTerminalLineService : IWtTerminalLineService
{
    private readonly IRepository<WtTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WtTerminalLineService(
        IRepository<WtTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(_mapper.Map<List<WtTerminalLineDto>>(items), _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WtTerminalLineDto>>.SuccessResult(new PagedResponse<WtTerminalLineDto>(_mapper.Map<List<WtTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtTerminalLineNotFound");
            return ApiResponse<WtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WtTerminalLineDto>.SuccessResult(_mapper.Map<WtTerminalLineDto>(entity), _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(_mapper.Map<List<WtTerminalLineDto>>(items), _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(_mapper.Map<List<WtTerminalLineDto>>(items), _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtTerminalLineDto>> CreateAsync(CreateWtTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WtTerminalLine>(createDto) ?? new WtTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtTerminalLineDto>.SuccessResult(_mapper.Map<WtTerminalLineDto>(entity), _localizationService.GetLocalizedString("WtTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WtTerminalLineDto>> UpdateAsync(long id, UpdateWtTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WtTerminalLineNotFound");
            return ApiResponse<WtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtTerminalLineDto>.SuccessResult(_mapper.Map<WtTerminalLineDto>(entity), _localizationService.GetLocalizedString("WtTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WtTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtTerminalLineDeletedSuccessfully"));
    }
}

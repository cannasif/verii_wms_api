using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtTerminalLineService : IPtTerminalLineService
{
    private readonly IRepository<PtTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PtTerminalLineService(
        IRepository<PtTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(_mapper.Map<List<PtTerminalLineDto>>(items), _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PtTerminalLineDto>>.SuccessResult(new PagedResponse<PtTerminalLineDto>(_mapper.Map<List<PtTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtTerminalLineNotFound");
            return ApiResponse<PtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PtTerminalLineDto>.SuccessResult(_mapper.Map<PtTerminalLineDto>(entity), _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(_mapper.Map<List<PtTerminalLineDto>>(items), _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(_mapper.Map<List<PtTerminalLineDto>>(items), _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtTerminalLineDto>> CreateAsync(CreatePtTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PtTerminalLine>(createDto) ?? new PtTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtTerminalLineDto>.SuccessResult(_mapper.Map<PtTerminalLineDto>(entity), _localizationService.GetLocalizedString("PtTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtTerminalLineDto>> UpdateAsync(long id, UpdatePtTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PtTerminalLineNotFound");
            return ApiResponse<PtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtTerminalLineDto>.SuccessResult(_mapper.Map<PtTerminalLineDto>(entity), _localizationService.GetLocalizedString("PtTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PtTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtTerminalLineDeletedSuccessfully"));
    }
}

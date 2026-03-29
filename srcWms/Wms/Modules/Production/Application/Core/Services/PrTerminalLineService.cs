using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrTerminalLineService : IPrTerminalLineService
{
    private readonly IRepository<PrTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrTerminalLineService(
        IRepository<PrTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(_mapper.Map<List<PrTerminalLineDto>>(items), _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PrTerminalLineDto>>.SuccessResult(new PagedResponse<PrTerminalLineDto>(_mapper.Map<List<PrTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrTerminalLineNotFound");
            return ApiResponse<PrTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PrTerminalLineDto>.SuccessResult(_mapper.Map<PrTerminalLineDto>(entity), _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(_mapper.Map<List<PrTerminalLineDto>>(items), _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrTerminalLineDto>>.SuccessResult(_mapper.Map<List<PrTerminalLineDto>>(items), _localizationService.GetLocalizedString("PrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrTerminalLineDto>> CreateAsync(CreatePrTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<PrTerminalLine>(createDto) ?? new PrTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrTerminalLineDto>.SuccessResult(_mapper.Map<PrTerminalLineDto>(entity), _localizationService.GetLocalizedString("PrTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrTerminalLineDto>> UpdateAsync(long id, UpdatePrTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PrTerminalLineNotFound");
            return ApiResponse<PrTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrTerminalLineDto>.SuccessResult(_mapper.Map<PrTerminalLineDto>(entity), _localizationService.GetLocalizedString("PrTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("PrTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrTerminalLineDeletedSuccessfully"));
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrTerminalLineService : IGrTerminalLineService
{
    private readonly IRepository<GrTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public GrTerminalLineService(
        IRepository<GrTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(_mapper.Map<List<GrTerminalLineDto>>(items), _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<GrTerminalLineDto>>.SuccessResult(new PagedResponse<GrTerminalLineDto>(_mapper.Map<List<GrTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrTerminalLineNotFound");
            return ApiResponse<GrTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<GrTerminalLineDto>.SuccessResult(_mapper.Map<GrTerminalLineDto>(entity), _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(_mapper.Map<List<GrTerminalLineDto>>(items), _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(_mapper.Map<List<GrTerminalLineDto>>(items), _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrTerminalLineDto>> CreateAsync(CreateGrTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GrTerminalLine>(createDto) ?? new GrTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrTerminalLineDto>.SuccessResult(_mapper.Map<GrTerminalLineDto>(entity), _localizationService.GetLocalizedString("GrTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrTerminalLineDto>> UpdateAsync(long id, UpdateGrTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("GrTerminalLineNotFound");
            return ApiResponse<GrTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrTerminalLineDto>.SuccessResult(_mapper.Map<GrTerminalLineDto>(entity), _localizationService.GetLocalizedString("GrTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("GrTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrTerminalLineDeletedSuccessfully"));
    }
}

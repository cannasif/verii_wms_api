using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Services;

public sealed class ShTerminalLineService : IShTerminalLineService
{
    private readonly IRepository<ShTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public ShTerminalLineService(
        IRepository<ShTerminalLine> terminalLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IMapper mapper)
    {
        _terminalLines = terminalLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(_mapper.Map<List<ShTerminalLineDto>>(items), _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ShTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<ShTerminalLineDto>>.SuccessResult(new PagedResponse<ShTerminalLineDto>(_mapper.Map<List<ShTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShTerminalLineNotFound");
            return ApiResponse<ShTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<ShTerminalLineDto>.SuccessResult(_mapper.Map<ShTerminalLineDto>(entity), _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(_mapper.Map<List<ShTerminalLineDto>>(items), _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(_mapper.Map<List<ShTerminalLineDto>>(items), _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShTerminalLineDto>> CreateAsync(CreateShTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ShTerminalLine>(createDto) ?? new ShTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShTerminalLineDto>.SuccessResult(_mapper.Map<ShTerminalLineDto>(entity), _localizationService.GetLocalizedString("ShTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ShTerminalLineDto>> UpdateAsync(long id, UpdateShTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("ShTerminalLineNotFound");
            return ApiResponse<ShTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShTerminalLineDto>.SuccessResult(_mapper.Map<ShTerminalLineDto>(entity), _localizationService.GetLocalizedString("ShTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("ShTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShTerminalLineDeletedSuccessfully"));
    }
}

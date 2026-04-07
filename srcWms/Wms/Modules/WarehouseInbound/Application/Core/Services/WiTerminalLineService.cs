using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Services;

public sealed class WiTerminalLineService : IWiTerminalLineService
{
    private readonly IRepository<WiTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public WiTerminalLineService(
        IRepository<WiTerminalLine> terminalLines,
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

    public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(_mapper.Map<List<WiTerminalLineDto>>(items), _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WiTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WiTerminalLineDto>>.SuccessResult(new PagedResponse<WiTerminalLineDto>(_mapper.Map<List<WiTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiTerminalLineNotFound");
            return ApiResponse<WiTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WiTerminalLineDto>.SuccessResult(_mapper.Map<WiTerminalLineDto>(entity), _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(_mapper.Map<List<WiTerminalLineDto>>(items), _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(_mapper.Map<List<WiTerminalLineDto>>(items), _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiTerminalLineDto>> CreateAsync(CreateWiTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<WiTerminalLine>(createDto) ?? new WiTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiTerminalLineDto>.SuccessResult(_mapper.Map<WiTerminalLineDto>(entity), _localizationService.GetLocalizedString("WiTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WiTerminalLineDto>> UpdateAsync(long id, UpdateWiTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("WiTerminalLineNotFound");
            return ApiResponse<WiTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiTerminalLineDto>.SuccessResult(_mapper.Map<WiTerminalLineDto>(entity), _localizationService.GetLocalizedString("WiTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("WiTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiTerminalLineDeletedSuccessfully"));
    }
}

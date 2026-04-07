using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtTerminalLineService : ISrtTerminalLineService
{
    private readonly IRepository<SrtTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public SrtTerminalLineService(
        IRepository<SrtTerminalLine> terminalLines,
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

    public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(_mapper.Map<List<SrtTerminalLineDto>>(items), _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SrtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SrtTerminalLineDto>>.SuccessResult(new PagedResponse<SrtTerminalLineDto>(_mapper.Map<List<SrtTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtTerminalLineNotFound");
            return ApiResponse<SrtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SrtTerminalLineDto>.SuccessResult(_mapper.Map<SrtTerminalLineDto>(entity), _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(_mapper.Map<List<SrtTerminalLineDto>>(items), _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(_mapper.Map<List<SrtTerminalLineDto>>(items), _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtTerminalLineDto>> CreateAsync(CreateSrtTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SrtTerminalLine>(createDto) ?? new SrtTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtTerminalLineDto>.SuccessResult(_mapper.Map<SrtTerminalLineDto>(entity), _localizationService.GetLocalizedString("SrtTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SrtTerminalLineDto>> UpdateAsync(long id, UpdateSrtTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SrtTerminalLineNotFound");
            return ApiResponse<SrtTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtTerminalLineDto>.SuccessResult(_mapper.Map<SrtTerminalLineDto>(entity), _localizationService.GetLocalizedString("SrtTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SrtTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtTerminalLineDeletedSuccessfully"));
    }
}

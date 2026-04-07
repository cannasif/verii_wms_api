using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitTerminalLineService : ISitTerminalLineService
{
    private readonly IRepository<SitTerminalLine> _terminalLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public SitTerminalLineService(
        IRepository<SitTerminalLine> terminalLines,
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

    public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(_mapper.Map<List<SitTerminalLineDto>>(items), _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SitTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _terminalLines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SitTerminalLineDto>>.SuccessResult(new PagedResponse<SitTerminalLineDto>(_mapper.Map<List<SitTerminalLineDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitTerminalLineNotFound");
            return ApiResponse<SitTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SitTerminalLineDto>.SuccessResult(_mapper.Map<SitTerminalLineDto>(entity), _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(_mapper.Map<List<SitTerminalLineDto>>(items), _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var items = await _terminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(_mapper.Map<List<SitTerminalLineDto>>(items), _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitTerminalLineDto>> CreateAsync(CreateSitTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SitTerminalLine>(createDto) ?? new SitTerminalLine();
        entity.CreatedDate = DateTimeProvider.Now;
        await _terminalLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitTerminalLineDto>.SuccessResult(_mapper.Map<SitTerminalLineDto>(entity), _localizationService.GetLocalizedString("SitTerminalLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SitTerminalLineDto>> UpdateAsync(long id, UpdateSitTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _terminalLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("SitTerminalLineNotFound");
            return ApiResponse<SitTerminalLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        entity.UpdatedDate = DateTimeProvider.Now;
        _terminalLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitTerminalLineDto>.SuccessResult(_mapper.Map<SitTerminalLineDto>(entity), _localizationService.GetLocalizedString("SitTerminalLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _terminalLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var msg = _localizationService.GetLocalizedString("SitTerminalLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _terminalLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitTerminalLineDeletedSuccessfully"));
    }
}

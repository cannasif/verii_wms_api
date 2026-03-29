using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public sealed class SitLineSerialService : ISitLineSerialService
{
    private readonly IRepository<SitLineSerial> _serials;
    private readonly IRepository<SitLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public SitLineSerialService(
        IRepository<SitLineSerial> serials,
        IRepository<SitLine> lines,
        IUnitOfWork unitOfSitrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfSitrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(_mapper.Map<List<SitLineSerialDto>>(items), _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SitLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SitLineSerialDto>>.SuccessResult(new PagedResponse<SitLineSerialDto>(_mapper.Map<List<SitLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SitLineSerialNotFound");
            return ApiResponse<SitLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SitLineSerialDto>.SuccessResult(_mapper.Map<SitLineSerialDto>(entity), _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(_mapper.Map<List<SitLineSerialDto>>(items), _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("SitLineNotFound");
            return ApiResponse<SitLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<SitLineSerial>(createDto) ?? new SitLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitLineSerialDto>.SuccessResult(_mapper.Map<SitLineSerialDto>(entity), _localizationService.GetLocalizedString("SitLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SitLineSerialNotFound");
            return ApiResponse<SitLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("SitLineNotFound");
            return ApiResponse<SitLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SitLineSerialDto>.SuccessResult(_mapper.Map<SitLineSerialDto>(entity), _localizationService.GetLocalizedString("SitLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SitLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.ProductionTransfer.Services;

public sealed class PtLineSerialService : IPtLineSerialService
{
    private readonly IRepository<PtLineSerial> _serials;
    private readonly IRepository<PtLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PtLineSerialService(
        IRepository<PtLineSerial> serials,
        IRepository<PtLine> lines,
        IUnitOfWork unitOfPtrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfPtrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(_mapper.Map<List<PtLineSerialDto>>(items), _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PtLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PtLineSerialDto>>.SuccessResult(new PagedResponse<PtLineSerialDto>(_mapper.Map<List<PtLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PtLineSerialNotFound");
            return ApiResponse<PtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PtLineSerialDto>.SuccessResult(_mapper.Map<PtLineSerialDto>(entity), _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(_mapper.Map<List<PtLineSerialDto>>(items), _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineSerialDto>> CreateAsync(CreatePtLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("PtLineNotFound");
            return ApiResponse<PtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<PtLineSerial>(createDto) ?? new PtLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtLineSerialDto>.SuccessResult(_mapper.Map<PtLineSerialDto>(entity), _localizationService.GetLocalizedString("PtLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PtLineSerialDto>> UpdateAsync(long id, UpdatePtLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PtLineSerialNotFound");
            return ApiResponse<PtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("PtLineNotFound");
            return ApiResponse<PtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PtLineSerialDto>.SuccessResult(_mapper.Map<PtLineSerialDto>(entity), _localizationService.GetLocalizedString("PtLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PtLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

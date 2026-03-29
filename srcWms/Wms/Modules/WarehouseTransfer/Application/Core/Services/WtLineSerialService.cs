using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.WarehouseTransfer.Services;

public sealed class WtLineSerialService : IWtLineSerialService
{
    private readonly IRepository<WtLineSerial> _serials;
    private readonly IRepository<WtLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WtLineSerialService(
        IRepository<WtLineSerial> serials,
        IRepository<WtLine> lines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(_mapper.Map<List<WtLineSerialDto>>(items), _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WtLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WtLineSerialDto>>.SuccessResult(new PagedResponse<WtLineSerialDto>(_mapper.Map<List<WtLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WtLineSerialNotFound");
            return ApiResponse<WtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WtLineSerialDto>.SuccessResult(_mapper.Map<WtLineSerialDto>(entity), _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(_mapper.Map<List<WtLineSerialDto>>(items), _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WtLineSerialDto>> CreateAsync(CreateWtLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WtLineNotFound");
            return ApiResponse<WtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<WtLineSerial>(createDto) ?? new WtLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtLineSerialDto>.SuccessResult(_mapper.Map<WtLineSerialDto>(entity), _localizationService.GetLocalizedString("WtLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WtLineSerialDto>> UpdateAsync(long id, UpdateWtLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WtLineSerialNotFound");
            return ApiResponse<WtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WtLineNotFound");
            return ApiResponse<WtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WtLineSerialDto>.SuccessResult(_mapper.Map<WtLineSerialDto>(entity), _localizationService.GetLocalizedString("WtLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WtLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

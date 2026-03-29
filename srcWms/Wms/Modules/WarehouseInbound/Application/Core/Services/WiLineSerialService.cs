using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Domain.Entities.WarehouseInbound;

namespace Wms.Application.WarehouseInbound.Services;

public sealed class WiLineSerialService : IWiLineSerialService
{
    private readonly IRepository<WiLineSerial> _serials;
    private readonly IRepository<WiLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public WiLineSerialService(
        IRepository<WiLineSerial> serials,
        IRepository<WiLine> lines,
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

    public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(_mapper.Map<List<WiLineSerialDto>>(items), _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WiLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WiLineSerialDto>>.SuccessResult(new PagedResponse<WiLineSerialDto>(_mapper.Map<List<WiLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WiLineSerialNotFound");
            return ApiResponse<WiLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WiLineSerialDto>.SuccessResult(_mapper.Map<WiLineSerialDto>(entity), _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(_mapper.Map<List<WiLineSerialDto>>(items), _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WiLineNotFound");
            return ApiResponse<WiLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<WiLineSerial>(createDto) ?? new WiLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiLineSerialDto>.SuccessResult(_mapper.Map<WiLineSerialDto>(entity), _localizationService.GetLocalizedString("WiLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WiLineSerialNotFound");
            return ApiResponse<WiLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WiLineNotFound");
            return ApiResponse<WiLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WiLineSerialDto>.SuccessResult(_mapper.Map<WiLineSerialDto>(entity), _localizationService.GetLocalizedString("WiLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WiLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

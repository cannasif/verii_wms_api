using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Domain.Entities.Shipping;

namespace Wms.Application.Shipping.Services;

public sealed class ShLineSerialService : IShLineSerialService
{
    private readonly IRepository<ShLineSerial> _serials;
    private readonly IRepository<ShLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public ShLineSerialService(
        IRepository<ShLineSerial> serials,
        IRepository<ShLine> lines,
        IUnitOfWork unitOfShrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfShrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(_mapper.Map<List<ShLineSerialDto>>(items), _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ShLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<ShLineSerialDto>>.SuccessResult(new PagedResponse<ShLineSerialDto>(_mapper.Map<List<ShLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("ShLineSerialNotFound");
            return ApiResponse<ShLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<ShLineSerialDto>.SuccessResult(_mapper.Map<ShLineSerialDto>(entity), _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ShLineSerialDto>>.SuccessResult(_mapper.Map<List<ShLineSerialDto>>(items), _localizationService.GetLocalizedString("ShLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineSerialDto>> CreateAsync(CreateShLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("ShLineNotFound");
            return ApiResponse<ShLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<ShLineSerial>(createDto) ?? new ShLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShLineSerialDto>.SuccessResult(_mapper.Map<ShLineSerialDto>(entity), _localizationService.GetLocalizedString("ShLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ShLineSerialDto>> UpdateAsync(long id, UpdateShLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("ShLineSerialNotFound");
            return ApiResponse<ShLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("ShLineNotFound");
            return ApiResponse<ShLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<ShLineSerialDto>.SuccessResult(_mapper.Map<ShLineSerialDto>(entity), _localizationService.GetLocalizedString("ShLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("ShLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

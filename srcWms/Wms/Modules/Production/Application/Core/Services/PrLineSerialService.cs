using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Services;

public sealed class PrLineSerialService : IPrLineSerialService
{
    private readonly IRepository<PrLineSerial> _serials;
    private readonly IRepository<PrLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;

    public PrLineSerialService(
        IRepository<PrLineSerial> serials,
        IRepository<PrLine> lines,
        IUnitOfWork unitOfPrrk,
        ILocalizationService localizationService,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfPrrk;
        _localizationService = localizationService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(_mapper.Map<List<PrLineSerialDto>>(items), _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PrLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<PrLineSerialDto>>.SuccessResult(new PagedResponse<PrLineSerialDto>(_mapper.Map<List<PrLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PrLineSerialNotFound");
            return ApiResponse<PrLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PrLineSerialDto>.SuccessResult(_mapper.Map<PrLineSerialDto>(entity), _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PrLineSerialDto>>.SuccessResult(_mapper.Map<List<PrLineSerialDto>>(items), _localizationService.GetLocalizedString("PrLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineSerialDto>> CreateAsync(CreatePrLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("PrLineNotFound");
            return ApiResponse<PrLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<PrLineSerial>(createDto) ?? new PrLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrLineSerialDto>.SuccessResult(_mapper.Map<PrLineSerialDto>(entity), _localizationService.GetLocalizedString("PrLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PrLineSerialDto>> UpdateAsync(long id, UpdatePrLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PrLineSerialNotFound");
            return ApiResponse<PrLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("PrLineNotFound");
            return ApiResponse<PrLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PrLineSerialDto>.SuccessResult(_mapper.Map<PrLineSerialDto>(entity), _localizationService.GetLocalizedString("PrLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PrLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

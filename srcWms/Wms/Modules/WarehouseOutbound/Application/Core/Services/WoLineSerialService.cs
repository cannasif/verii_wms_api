using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Domain.Entities.WarehouseOutbound;

namespace Wms.Application.WarehouseOutbound.Services;

public sealed class WoLineSerialService : IWoLineSerialService
{
    private readonly IRepository<WoLineSerial> _serials;
    private readonly IRepository<WoLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public WoLineSerialService(
        IRepository<WoLineSerial> serials,
        IRepository<WoLine> lines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<WoLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<WoLineSerialDto>>.SuccessResult(new PagedResponse<WoLineSerialDto>(_mapper.Map<List<WoLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WoLineSerialNotFound");
            return ApiResponse<WoLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<WoLineSerialDto>.SuccessResult(_mapper.Map<WoLineSerialDto>(entity), _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<WoLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineSerialDto>> CreateAsync(CreateWoLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WoLineNotFound");
            return ApiResponse<WoLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<WoLineSerial>(createDto) ?? new WoLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoLineSerialDto>.SuccessResult(_mapper.Map<WoLineSerialDto>(entity), _localizationService.GetLocalizedString("WoLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<WoLineSerialDto>> UpdateAsync(long id, UpdateWoLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WoLineSerialNotFound");
            return ApiResponse<WoLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("WoLineNotFound");
            return ApiResponse<WoLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<WoLineSerialDto>.SuccessResult(_mapper.Map<WoLineSerialDto>(entity), _localizationService.GetLocalizedString("WoLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("WoLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

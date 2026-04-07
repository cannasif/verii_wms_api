using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public sealed class SrtLineSerialService : ISrtLineSerialService
{
    private readonly IRepository<SrtLineSerial> _serials;
    private readonly IRepository<SrtLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public SrtLineSerialService(
        IRepository<SrtLineSerial> serials,
        IRepository<SrtLine> lines,
        IUnitOfWork unitOfSrtrk,
        ILocalizationService localizationService,
        IDocumentReferenceReadEnricher documentReferenceReadEnricher,
        IMapper mapper)
    {
        _serials = serials;
        _lines = lines;
        _unitOfWork = unitOfSrtrk;
        _localizationService = localizationService;
        _documentReferenceReadEnricher = documentReferenceReadEnricher;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<SrtLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _serials.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<SrtLineSerialDto>>.SuccessResult(new PagedResponse<SrtLineSerialDto>(_mapper.Map<List<SrtLineSerialDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineSerialNotFound");
            return ApiResponse<SrtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<SrtLineSerialDto>.SuccessResult(_mapper.Map<SrtLineSerialDto>(entity), _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<SrtLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineSerialDto>> CreateAsync(CreateSrtLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (!await _lines.ExistsAsync(createDto.LineId, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("SrtLineNotFound");
            return ApiResponse<SrtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<SrtLineSerial>(createDto) ?? new SrtLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtLineSerialDto>.SuccessResult(_mapper.Map<SrtLineSerialDto>(entity), _localizationService.GetLocalizedString("SrtLineSerialCreatedSuccessfully"));
    }

    public async Task<ApiResponse<SrtLineSerialDto>> UpdateAsync(long id, UpdateSrtLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineSerialNotFound");
            return ApiResponse<SrtLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("SrtLineNotFound");
            return ApiResponse<SrtLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<SrtLineSerialDto>.SuccessResult(_mapper.Map<SrtLineSerialDto>(entity), _localizationService.GetLocalizedString("SrtLineSerialUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("SrtLineSerialNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtLineSerialDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

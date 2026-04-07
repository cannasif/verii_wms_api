using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Domain.Entities.GoodsReceipt;

namespace Wms.Application.GoodsReceipt.Services;

public sealed class GrLineSerialService : IGrLineSerialService
{
    private readonly IRepository<GrLineSerial> _serials;
    private readonly IRepository<GrLine> _lines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IDocumentReferenceReadEnricher _documentReferenceReadEnricher;
    private readonly IMapper _mapper;

    public GrLineSerialService(
        IRepository<GrLineSerial> serials,
        IRepository<GrLine> lines,
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

    public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        return await GetPagedAsync(request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize, request.SortBy, request.SortDirection, cancellationToken);
    }

    public async Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc", CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _serials.Query()
            .ApplySorting(string.IsNullOrWhiteSpace(sortBy) ? "Id" : sortBy.Trim(), string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<GrLineSerialDto>>.SuccessResult(new PagedResponse<GrLineSerialDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
            return ApiResponse<GrLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<GrLineSerialDto>.SuccessResult(_mapper.Map<GrLineSerialDto>(entity), _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _serials.Query().Where(x => x.LineId == lineId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<GrLineSerialDto>>(items);
        await _documentReferenceReadEnricher.EnrichLineSerialsAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineSerialDto>> CreateAsync(CreateGrLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        if (createDto.LineId.HasValue && !await _lines.ExistsAsync(createDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("GrLineNotFound");
            return ApiResponse<GrLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<GrLineSerial>(createDto) ?? new GrLineSerial();
        await _serials.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrLineSerialDto>.SuccessResult(_mapper.Map<GrLineSerialDto>(entity), _localizationService.GetLocalizedString("GrImportSerialLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<GrLineSerialDto>> UpdateAsync(long id, UpdateGrLineSerialDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
            return ApiResponse<GrLineSerialDto>.ErrorResult(msg, msg, 404);
        }

        if (updateDto.LineId.HasValue && !await _lines.ExistsAsync(updateDto.LineId.Value, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("GrLineNotFound");
            return ApiResponse<GrLineSerialDto>.ErrorResult(msg, msg, 400);
        }

        _mapper.Map(updateDto, entity);
        _serials.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<GrLineSerialDto>.SuccessResult(_mapper.Map<GrLineSerialDto>(entity), _localizationService.GetLocalizedString("GrImportSerialLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serials.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _serials.SoftDelete(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportSerialLineDeletedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

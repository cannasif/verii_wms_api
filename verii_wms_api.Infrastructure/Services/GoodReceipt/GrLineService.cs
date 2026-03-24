using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrLineService : IGrLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GrLineService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<PagedResponse<GrLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.GrLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<GrLineDto>>(items);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<PagedResponse<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
dtos = enriched.Data?.ToList() ?? dtos;

var result = new PagedResponse<GrLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<GrLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.GrLines.Query().ToListAsync(requestCancellationToken);
var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}

return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var line = await _unitOfWork.GrLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (line == null)
{
    return ApiResponse<GrLineDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrLineNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrLineMissing"));
}

var lineDto = _mapper.Map<GrLineDto>(line);

var enriched = await _erpService.PopulateStockNamesAsync(new[] { lineDto });
if (!enriched.Success)
{
    return ApiResponse<GrLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
var finalDto = enriched.Data?.FirstOrDefault() ?? lineDto;

return ApiResponse<GrLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.GrLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}

return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrLineDto>> CreateAsync(CreateGrLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
// Header'ın var olup olmadığını kontrol et
var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)createDto.HeaderId);
if (!headerExists)
{
    return ApiResponse<GrLineDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrHeaderNotFound"),
        _localizationService.GetLocalizedString("HeaderNotFound"),
        400,
        _localizationService.GetLocalizedString("GrHeaderMissing"));
}

var line = _mapper.Map<GrLine>(createDto);

await _unitOfWork.GrLines.AddAsync(line);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var lineDto = _mapper.Map<GrLineDto>(line);
return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<GrLineDto>> UpdateAsync(long id, UpdateGrLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existingLine = await _unitOfWork.GrLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existingLine == null)
{
    return ApiResponse<GrLineDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrLineNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrLineMissing"));
}

// Header'ın var olup olmadığını kontrol et
var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)updateDto.HeaderId);
if (!headerExists)
{
    return ApiResponse<GrLineDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrHeaderNotFound"),
        _localizationService.GetLocalizedString("HeaderNotFound"),
        400,
        _localizationService.GetLocalizedString("GrHeaderMissing"));
}

_mapper.Map(updateDto, existingLine);

_unitOfWork.GrLines.Update(existingLine);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var lineDto = _mapper.Map<GrLineDto>(existingLine);
return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.GrLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("GrLineNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrLineMissing"));
}

var hasActiveLineSerials = await _unitOfWork.GrLineSerials
    .Query()
    .Where(ls => ls.LineId == id)
            .AnyAsync(requestCancellationToken);
if (hasActiveLineSerials)
{
    var msg = _localizationService.GetLocalizedString("GrLineLineSerialsExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var importLines = await _unitOfWork.GrImportLines.Query().Where(x => x.LineId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("GrLineImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var headerId = entity.HeaderId;
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.GrLines.SoftDelete(id);

    var hasOtherLines = await _unitOfWork.GrLines
        .Query()
        .Where(l => l.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    var hasOtherImportLines = await _unitOfWork.GrImportLines
        .Query()
        .Where(il => il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    if (!hasOtherLines && !hasOtherImportLines)
    {
        await _unitOfWork.GrHeaders.SoftDelete(headerId);
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrLineDeletedSuccessfully"));
        }

        

        // GrHeader ilişkili satırları (GrLine) headerId’ye göre getirir
        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetLinesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.GrLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
        }
        
    }
}

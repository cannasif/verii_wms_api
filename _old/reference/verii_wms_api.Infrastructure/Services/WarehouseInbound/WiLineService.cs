using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiLineService : IWiLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WiLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WiLines.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WiLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WiLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.WiLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<WiLineDto>>(items);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<PagedResponse<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
dtos = enriched.Data?.ToList() ?? dtos;
var result = new PagedResponse<WiLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<WiLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<WiLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WiLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null) return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
var dto = _mapper.Map<WiLineDto>(entity);
var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
if (!enriched.Success)
{
    return ApiResponse<WiLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
return ApiResponse<WiLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WiLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WiLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WiLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<WiLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<WiLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WiLineRetrievedSuccessfully"));
        }

        



        public async Task<ApiResponse<WiLineDto>> CreateAsync(CreateWiLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WiLine>(createDto);
var created = await _unitOfWork.WiLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WiLineDto>(created);
return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WiLineDto>> UpdateAsync(long id, UpdateWiLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existing = await _unitOfWork.WiLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existing == null) return ApiResponse<WiLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
var entity = _mapper.Map(updateDto, existing);
_unitOfWork.WiLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WiLineDto>(entity);
return ApiResponse<WiLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WiLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 404);
}

var hasActiveLineSerials = await _unitOfWork.WiLineSerials
    .Query()
    .Where(ls => ls.LineId == id)
            .AnyAsync(requestCancellationToken);
if (hasActiveLineSerials)
{
    var msg = _localizationService.GetLocalizedString("WiLineLineSerialsExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var importLines = await _unitOfWork.WiImportLines.Query().Where(x => x.LineId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("WiLineImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var headerId = entity.HeaderId;
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.WiLines.SoftDelete(id);

    var hasOtherLines = await _unitOfWork.WiLines
        .Query()
        .Where(l => l.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    var hasOtherImportLines = await _unitOfWork.WiImportLines
        .Query()
        .Where(il => il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    if (!hasOtherLines && !hasOtherImportLines)
    {
        await _unitOfWork.WiHeaders.SoftDelete(headerId);
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiLineDeletedSuccessfully"));
        }
    }
}

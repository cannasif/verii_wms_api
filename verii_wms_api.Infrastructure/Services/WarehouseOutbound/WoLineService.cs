using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoLineService : IWoLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WoLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoLines.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WoLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WoLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.WoLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<WoLineDto>>(items);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<PagedResponse<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
dtos = enriched.Data?.ToList() ?? dtos;
var result = new PagedResponse<WoLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<WoLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<WoLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WoLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null) return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
var dto = _mapper.Map<WoLineDto>(entity);
var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
if (!enriched.Success)
{
    return ApiResponse<WoLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
return ApiResponse<WoLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WoLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<WoLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<WoLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("WoLineRetrievedSuccessfully"));
        }

        



        public async Task<ApiResponse<WoLineDto>> CreateAsync(CreateWoLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WoLine>(createDto);
var created = await _unitOfWork.WoLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoLineDto>(created);
return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WoLineDto>> UpdateAsync(long id, UpdateWoLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existing = await _unitOfWork.WoLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existing == null) return ApiResponse<WoLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
var entity = _mapper.Map(updateDto, existing);
_unitOfWork.WoLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoLineDto>(entity);
return ApiResponse<WoLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WoLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 404);
}

var hasActiveLineSerials = await _unitOfWork.WoLineSerials
    .Query()
    .Where(ls => ls.LineId == id)
            .AnyAsync(requestCancellationToken);
if (hasActiveLineSerials)
{
    var msg = _localizationService.GetLocalizedString("WoLineLineSerialsExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var importLines = await _unitOfWork.WoImportLines.Query().Where(x => x.LineId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("WoLineImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var headerId = entity.HeaderId;
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.WoLines.SoftDelete(id);

    var hasOtherLines = await _unitOfWork.WoLines
        .Query()
        .Where(l => l.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    var hasOtherImportLines = await _unitOfWork.WoImportLines
        .Query()
        .Where(il => il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    if (!hasOtherLines && !hasOtherImportLines)
    {
        await _unitOfWork.WoHeaders.SoftDelete(headerId);
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoLineDeletedSuccessfully"));
        }
    }
}

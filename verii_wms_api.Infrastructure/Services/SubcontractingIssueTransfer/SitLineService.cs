using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitLineService : ISitLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SitLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<PagedResponse<SitLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.SitLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SitLineDto>>(items);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<PagedResponse<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
dtos = enriched.Data?.ToList() ?? dtos;
var result = new PagedResponse<SitLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SitLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitLines.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
}
var dto = _mapper.Map<SitLineDto>(entity);
var enriched = await _erpService.PopulateStockNamesAsync(new[] { dto });
if (!enriched.Success)
{
    return ApiResponse<SitLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
var finalDto = enriched.Data?.FirstOrDefault() ?? dto;
return ApiResponse<SitLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitLineDto>>(entities);
var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<SitLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
return ApiResponse<IEnumerable<SitLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("SitLineRetrievedSuccessfully"));
        }

        



        public async Task<ApiResponse<SitLineDto>> CreateAsync(CreateSitLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<SitLine>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.SitLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitLineDto>(entity);
return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SitLineDto>> UpdateAsync(long id, UpdateSitLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<SitLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.SitLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitLineDto>(entity);
return ApiResponse<SitLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 404);
}

var hasActiveLineSerials = await _unitOfWork.SitLineSerials
    .Query()
    .Where(ls => ls.LineId == id)
            .AnyAsync(requestCancellationToken);
if (hasActiveLineSerials)
{
    var msg = _localizationService.GetLocalizedString("SitLineLineSerialsExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var importLines = await _unitOfWork.SitImportLines.Query().Where(x => x.LineId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("SitLineImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var headerId = entity.HeaderId;
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.SitLines.SoftDelete(id);

    var hasOtherLines = await _unitOfWork.SitLines.Query()
        .Where(l => !l.IsDeleted && l.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    var hasOtherImportLines = await _unitOfWork.SitImportLines.Query()
        .Where(il => !il.IsDeleted && il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    if (!hasOtherLines && !hasOtherImportLines)
    {
        await _unitOfWork.SitHeaders.SoftDelete(headerId);
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitLineDeletedSuccessfully"));
        }
    }
}

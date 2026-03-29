using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtLineService : IWtLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WtLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<PagedResponse<WtLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.WtLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<WtLineDto>>(items);

var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
if (!enrichedStock.Success)
{
    return ApiResponse<PagedResponse<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
}
dtos = enrichedStock.Data?.ToList() ?? dtos;

var result = new PagedResponse<WtLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<WtLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtLines.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
if (!enrichedStock.Success)
{
    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
}
dtos = enrichedStock.Data ?? dtos;

return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<WtLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WtLines
    .Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);

if (entity == null || entity.IsDeleted)
{
    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
}

var dto = _mapper.Map<WtLineDto>(entity);

var enrichedStock = await _erpService.PopulateStockNamesAsync(new[] { dto });
if (!enrichedStock.Success)
{
    return ApiResponse<WtLineDto>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
}
var finalDto = enrichedStock.Data?.FirstOrDefault() ?? dto;

return ApiResponse<WtLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WtLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WtLineDto>>(entities);

var enrichedStock = await _erpService.PopulateStockNamesAsync(dtos);
if (!enrichedStock.Success)
{
    return ApiResponse<IEnumerable<WtLineDto>>.ErrorResult(enrichedStock.Message, enrichedStock.ExceptionMessage, enrichedStock.StatusCode);
}
dtos = enrichedStock.Data ?? dtos;

return ApiResponse<IEnumerable<WtLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<WtLineDto>> CreateAsync(CreateWtLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WtLine>(createDto);
entity.IsDeleted = false;

await _unitOfWork.WtLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<WtLineDto>(entity);
return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WtLineDto>> UpdateAsync(long id, UpdateWtLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WtLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<WtLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
}

_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;

_unitOfWork.WtLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<WtLineDto>(entity);
return ApiResponse<WtLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WtLines.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 404);
}

var hasActiveLineSerials = await _unitOfWork.WtLineSerials
    .Query()
    .Where(ls => ls.LineId == id)
            .AnyAsync(requestCancellationToken);
if (hasActiveLineSerials)
{
    var msg = _localizationService.GetLocalizedString("WtLineLineSerialsExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var importLines = await _unitOfWork.WtImportLines.Query().Where(x => x.LineId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("WtLineImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var headerId = entity.HeaderId;
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.WtLines.SoftDelete(id);

    var hasOtherLines = await _unitOfWork.WtLines
        .Query()
        .Where(l => l.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    var hasOtherImportLines = await _unitOfWork.WtImportLines
        .Query()
        .Where(il => il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
    if (!hasOtherLines && !hasOtherImportLines)
    {
        await _unitOfWork.WtHeaders.SoftDelete(headerId);
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtLineDeletedSuccessfully"));
        }

    }
}

using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class SitLineSerialService : ISitLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SitLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var items = await _unitOfWork.SitLineSerials.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SitLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<SitLineSerialDto>>.ErrorResult(
        allResult.Message,
        allResult.ExceptionMessage,
        allResult.StatusCode);
}

var query = allResult.Data.AsQueryable();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = query.Count();
var items = query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToList();

var result = new PagedResponse<SitLineSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SitLineSerialDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
}
var dto = _mapper.Map<SitLineSerialDto>(entity);
return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var items = await _unitOfWork.SitLineSerials.Query().Where(x => x.LineId == lineId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lineExists = await _unitOfWork.SitLines.ExistsAsync(createDto.LineId);
if (!lineExists)
{
    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
}
var entity = _mapper.Map<SitLineSerial>(createDto);
entity.CreatedDate = DateTime.Now;
entity.IsDeleted = false;
await _unitOfWork.SitLineSerials.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitLineSerialDto>(entity);
return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
}
if (updateDto.LineId.HasValue)
{
    var lineExists = await _unitOfWork.SitLines.ExistsAsync(updateDto.LineId.Value);
    if (!lineExists)
    {
        return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
    }
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTime.Now;
_unitOfWork.SitLineSerials.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitLineSerialDto>(entity);
return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
}
var lineEntity = await _unitOfWork.SitLines.GetByIdAsync(entity.LineId);

{
    var s1 = (entity.SerialNo ?? "").Trim();
    var s2 = (entity.SerialNo2 ?? "").Trim();
    var s3 = (entity.SerialNo3 ?? "").Trim();
    var s4 = (entity.SerialNo4 ?? "").Trim();
    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
    if (anyEntitySerial)
    {
        var serialExistsInRoutes = await _unitOfWork.SitRoutes.Query()
            .Where(r => !r.IsDeleted
                           && r.ImportLine.LineId == entity.LineId
                           && (
                               (!string.IsNullOrWhiteSpace(s1) && (r.SerialNo ?? "").Trim() == s1) ||
                               (!string.IsNullOrWhiteSpace(s2) && (r.SerialNo2 ?? "").Trim() == s2) ||
                               (!string.IsNullOrWhiteSpace(s3) && (r.SerialNo3 ?? "").Trim() == s3) ||
                               (!string.IsNullOrWhiteSpace(s4) && (r.SerialNo4 ?? "").Trim() == s4)
                           ))
            .AnyAsync(requestCancellationToken);
        if (serialExistsInRoutes)
        {
            var msg = _localizationService.GetLocalizedString("SitLineSerialRoutesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }
    }
}

var totalLineSerialQty = await _unitOfWork.SitLineSerials.Query()
    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
    .SumAsync(ls => ls.Quantity);

var totalRouteQty = await _unitOfWork.SitRoutes.Query()
    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
    .SumAsync(r => r.Quantity);

var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
if (remainingAfterDelete < totalRouteQty)
{
    var msg = _localizationService.GetLocalizedString("SitLineSerialInsufficientQuantityAfterDelete");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}

var currentSerialCount = await _unitOfWork.SitLineSerials.Query()
    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
            .CountAsync(requestCancellationToken);
var remainingSerialCount = currentSerialCount - 1;

var hasImportLines = await _unitOfWork.SitImportLines.Query()
    .Where(il => !il.IsDeleted && il.LineId == entity.LineId)
            .AnyAsync(requestCancellationToken);
var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

var headerWillBeDeleted = false;
var headerIdToDelete = 0L;
if (lineWillBeDeleted && lineEntity != null)
{
    var headerId = lineEntity.HeaderId;
    var currentLinesUnderHeader = await _unitOfWork.SitLines.Query()
        .Where(l => !l.IsDeleted && l.HeaderId == headerId)
            .CountAsync(requestCancellationToken);
    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
    if (remainingLinesUnderHeader == 0)
    {
        var hasHeaderImportLines = await _unitOfWork.SitImportLines.Query()
            .Where(il => !il.IsDeleted && il.HeaderId == headerId)
            .AnyAsync(requestCancellationToken);
        if (!hasHeaderImportLines)
        {
            headerWillBeDeleted = true;
            headerIdToDelete = headerId;
        }
    }
}

using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.SitLineSerials.SoftDelete(id);

    if (lineWillBeDeleted)
    {
        await _unitOfWork.SitLines.SoftDelete(entity.LineId);
        if (headerWillBeDeleted && headerIdToDelete != 0)
        {
            await _unitOfWork.SitHeaders.SoftDelete(headerIdToDelete);
        }
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
    var msgKey = lineWillBeDeleted ? "SitLineSerialDeletedAndLineDeleted" : "SitLineSerialDeletedSuccessfully";
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}

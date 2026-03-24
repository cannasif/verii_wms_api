using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class GrLineSerialService : IGrLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GrLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc", CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (pageNumber < 1) pageNumber = 1;
if (pageSize < 1) pageSize = 10;

var query = _unitOfWork.GrLineSerials.Query();
sortBy = string.IsNullOrWhiteSpace(sortBy) ? "Id" : sortBy.Trim();
bool desc = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
query = query.ApplySorting(sortBy, desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<GrLineSerialDto>>(items);
var result = new PagedResponse<GrLineSerialDto>(dtos, totalCount, pageNumber, pageSize);
return ApiResponse<PagedResponse<GrLineSerialDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var serialLines = await _unitOfWork.GrLineSerials.GetAllAsync();
var serialLineDtos = _mapper.Map<IEnumerable<GrLineSerialDto>>(serialLines);
return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(serialLineDtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<GrLineSerialDto>>.ErrorResult(
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

var result = new PagedResponse<GrLineSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<GrLineSerialDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<GrLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var serialLine = await _unitOfWork.GrLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (serialLine == null)
{
    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
    return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 404);
}
var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var serialLines = await _unitOfWork.GrLineSerials.FindAsync(x => x.LineId == lineId);
var serialLineDtos = _mapper.Map<IEnumerable<GrLineSerialDto>>(serialLines);
return ApiResponse<IEnumerable<GrLineSerialDto>>.SuccessResult(serialLineDtos, _localizationService.GetLocalizedString("GrImportSerialLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrLineSerialDto>> CreateAsync(CreateGrLineSerialDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (createDto.LineId.HasValue)
{
    var lineExists = await _unitOfWork.GrLines.ExistsAsync(createDto.LineId.Value);
    if (!lineExists)
    {
        var nf = _localizationService.GetLocalizedString("GrLineNotFound");
        return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 400);
    }
}
var serialLine = _mapper.Map<GrLineSerial>(createDto);
await _unitOfWork.GrLineSerials.AddAsync(serialLine);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<GrLineSerialDto>> UpdateAsync(long id, UpdateGrLineSerialDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var serialLine = await _unitOfWork.GrLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (serialLine == null)
{
    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
    return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 404);
}
if (updateDto.LineId.HasValue)
{
    var lineExists = await _unitOfWork.GrLines.ExistsAsync(updateDto.LineId.Value);
    if (!lineExists)
    {
        var nf = _localizationService.GetLocalizedString("GrLineNotFound");
        return ApiResponse<GrLineSerialDto>.ErrorResult(nf, nf, 400);
    }
    serialLine.LineId = updateDto.LineId.Value;
}
_mapper.Map(updateDto, serialLine);
_unitOfWork.GrLineSerials.Update(serialLine);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var serialLineDto = _mapper.Map<GrLineSerialDto>(serialLine);
return ApiResponse<GrLineSerialDto>.SuccessResult(serialLineDto, _localizationService.GetLocalizedString("GrImportSerialLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var serialLine = await _unitOfWork.GrLineSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (serialLine == null)
{
    var nf = _localizationService.GetLocalizedString("GrImportSerialLineNotFound");
    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
}
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.GrLineSerials.SoftDelete(id);
    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportSerialLineDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }

        
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class SrtRouteService : ISrtRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SrtRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtRoutes.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SrtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<SrtRouteDto>>.ErrorResult(
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

var result = new PagedResponse<SrtRouteDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SrtRouteDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<SrtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SrtRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    var nf = _localizationService.GetLocalizedString("SrtRouteNotFound");
    return ApiResponse<SrtRouteDto>.ErrorResult(nf, nf, 404);
}
var dto = _mapper.Map<SrtRouteDto>(entity);
return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }

        

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetByStockCodeAsync(string stockCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.SrtRoutes.Query().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()) && !r.IsDeleted);
var entities = await query.ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var sn = (serialNo ?? "").Trim();
var entities = await _unitOfWork.SrtRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtRoutes.Query().Where(x => x.SourceWarehouse == sourceWarehouse).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtRoutes.Query().Where(x => x.TargetWarehouse == targetWarehouse).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtRouteDto>>(entities);
return ApiResponse<IEnumerable<SrtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtRouteRetrievedSuccessfully"));
        }



        public async Task<ApiResponse<SrtRouteDto>> CreateAsync(CreateSrtRouteDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<SrtRoute>(createDto);
await _unitOfWork.SrtRoutes.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SrtRouteDto>(entity);
return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SrtRouteDto>> UpdateAsync(long id, UpdateSrtRouteDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SrtRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<SrtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteNotFound"), _localizationService.GetLocalizedString("SrtRouteNotFound"), 404);
}
_mapper.Map(updateDto, entity);
_unitOfWork.SrtRoutes.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SrtRouteDto>(entity);
return ApiResponse<SrtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtRouteUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var route = await _unitOfWork.SrtRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (route == null || route.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtRouteNotFound"), _localizationService.GetLocalizedString("SrtRouteNotFound"), 404);
}

var importLineId = route.ImportLineId;

// Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
var remainingRoutesCount = await _unitOfWork.SrtRoutes.Query()
    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
            .CountAsync(requestCancellationToken);

// Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
var shouldDeleteImportLine = remainingRoutesCount == 0;

using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    // Route'u sil
    await _unitOfWork.SrtRoutes.SoftDelete(id);

    // Eğer bu son route ise, ImportLine'ı da sil
    if (shouldDeleteImportLine)
    {
        var importLine = await _unitOfWork.SrtImportLines.GetByIdAsync(importLineId);
        if (importLine != null && !importLine.IsDeleted)
        {
            await _unitOfWork.SrtImportLines.SoftDelete(importLineId);
        }
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtRouteDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}

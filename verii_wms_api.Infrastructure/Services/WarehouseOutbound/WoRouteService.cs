using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoRouteService : IWoRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WoRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoRoutes.GetAllAsync();
var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WoRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.WoRoutes.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<WoRouteDto>>(entities);
var result = new PagedResponse<WoRouteDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<WoRouteDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<WoRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WoRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null) { var nf = _localizationService.GetLocalizedString("WoRouteNotFound"); return ApiResponse<WoRouteDto>.ErrorResult(nf, nf, 404); }
var dto = _mapper.Map<WoRouteDto>(entity);
return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }

        

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByStockCodeAsync(string stockCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.WoRoutes.Query().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()));
var entities = await query.ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var sn = (serialNo ?? "").Trim();
var entities = await _unitOfWork.WoRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)));
var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse);
var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse);
var dtos = _mapper.Map<IEnumerable<WoRouteDto>>(entities);
return ApiResponse<IEnumerable<WoRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoRouteRetrievedSuccessfully"));
        }



        public async Task<ApiResponse<WoRouteDto>> CreateAsync(CreateWoRouteDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WoRoute>(createDto);
var created = await _unitOfWork.WoRoutes.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoRouteDto>(created);
return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WoRouteDto>> UpdateAsync(long id, UpdateWoRouteDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existing = await _unitOfWork.WoRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existing == null) { var nf = _localizationService.GetLocalizedString("WoRouteNotFound"); return ApiResponse<WoRouteDto>.ErrorResult(nf, nf, 404); }
var entity = _mapper.Map(updateDto, existing);
_unitOfWork.WoRoutes.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoRouteDto>(entity);
return ApiResponse<WoRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoRouteUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var route = await _unitOfWork.WoRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (route == null || route.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoRouteNotFound"), _localizationService.GetLocalizedString("WoRouteNotFound"), 404);
}

var importLineId = route.ImportLineId;

// Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
var remainingRoutesCount = await _unitOfWork.WoRoutes.Query()
    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
            .CountAsync(requestCancellationToken);

// Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
var shouldDeleteImportLine = remainingRoutesCount == 0;

using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    // Route'u sil
    await _unitOfWork.WoRoutes.SoftDelete(id);

    // Eğer bu son route ise, ImportLine'ı da sil
    if (shouldDeleteImportLine)
    {
        var importLine = await _unitOfWork.WoImportLines.GetByIdAsync(importLineId);
        if (importLine != null && !importLine.IsDeleted)
        {
            await _unitOfWork.WoImportLines.SoftDelete(importLineId);
        }
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoRouteDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}

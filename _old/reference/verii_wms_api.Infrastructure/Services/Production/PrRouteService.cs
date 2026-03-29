using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class PrRouteService : IPrRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetAllAsync()
        {
var entities = await _unitOfWork.PrRoutes.Query().ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<PrRouteDto>>> GetPagedAsync(PagedRequest request)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<PrRouteDto>>.ErrorResult(
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

var result = new PagedResponse<PrRouteDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<PrRouteDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<PrRouteDto>> GetByIdAsync(long id)
        {
var entity = await _unitOfWork.PrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
}
var dto = _mapper.Map<PrRouteDto>(entity);
return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
var entities = await _unitOfWork.PrRoutes.Query().Where(x => x.ImportLineId == importLineId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
var sn = (serialNo ?? "").Trim();
var entities = await _unitOfWork.PrRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
var entities = await _unitOfWork.PrRoutes.Query().Where(x => x.SourceWarehouse == sourceWarehouse).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PrRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
var entities = await _unitOfWork.PrRoutes.Query().Where(x => x.TargetWarehouse == targetWarehouse).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrRouteDto>>(entities);
return ApiResponse<IEnumerable<PrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrRouteRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<PrRouteDto>> CreateAsync(CreatePrRouteDto createDto)
        {
var entity = _mapper.Map<PrRoute>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.PrRoutes.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<PrRouteDto>(entity);
return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteCreatedSuccessfully"));
        }

        public async Task<ApiResponse<PrRouteDto>> UpdateAsync(long id, UpdatePrRouteDto updateDto)
        {
var entity = await _unitOfWork.PrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PrRouteDto>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.PrRoutes.Update(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<PrRouteDto>(entity);
return ApiResponse<PrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrRouteUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
var route = await _unitOfWork.PrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (route == null || route.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrRouteNotFound"), _localizationService.GetLocalizedString("PrRouteNotFound"), 404);
}

var importLineId = route.ImportLineId;

// Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
var remainingRoutesCount = await _unitOfWork.PrRoutes.Query()
    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
            .CountAsync();

// Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
var shouldDeleteImportLine = remainingRoutesCount == 0;

using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    // Route'u sil
    await _unitOfWork.PrRoutes.SoftDelete(id);

    // Eğer bu son route ise, ImportLine'ı da sil
    if (shouldDeleteImportLine)
    {
        var importLine = await _unitOfWork.PrImportLines.GetByIdAsync(importLineId);
        if (importLine != null && !importLine.IsDeleted)
        {
            await _unitOfWork.PrImportLines.SoftDelete(importLineId);
        }
    }

    await _unitOfWork.SaveChangesAsync();
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrRouteDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}

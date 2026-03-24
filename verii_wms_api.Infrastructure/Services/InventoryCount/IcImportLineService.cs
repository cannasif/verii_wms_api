using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class IcImportLineService : IIcImportLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public IcImportLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetAllAsync()
        {
var entities = await _unitOfWork.IcImportLines.Query().ToListAsync();
var dtos = _mapper.Map<IEnumerable<IcImportLineDto>>(entities);

var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}

return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<IcImportLineDto>>> GetPagedAsync(PagedRequest request)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<IcImportLineDto>>.ErrorResult(
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

var result = new PagedResponse<IcImportLineDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<IcImportLineDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<IcImportLineDto>> GetByIdAsync(long id)
        {
var entity = await _unitOfWork.IcImportLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
}
var dto = _mapper.Map<IcImportLineDto>(entity);
var enrichedSingle = await _erpService.PopulateStockNamesAsync(new[] { dto });
if (!enrichedSingle.Success)
{
    return ApiResponse<IcImportLineDto>.ErrorResult(enrichedSingle.Message, enrichedSingle.ExceptionMessage, enrichedSingle.StatusCode);
}
var finalDto = enrichedSingle.Data?.FirstOrDefault() ?? dto;
return ApiResponse<IcImportLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<IcImportLineDto>>> GetByHeaderIdAsync(long headerId)
        {
var entities = await _unitOfWork.IcImportLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<IcImportLineDto>>(entities);

var enriched = await _erpService.PopulateStockNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<IcImportLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}

return ApiResponse<IEnumerable<IcImportLineDto>>.SuccessResult(enriched.Data ?? dtos, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId)
        {
var header = await _unitOfWork.ICHeaders.GetByIdAsync(headerId);
if (header == null || header.IsDeleted)
{
    return ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
}

var importLines = await _unitOfWork.IcImportLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
var items = new List<IcImportLineWithRoutesDto>();

foreach (var il in importLines)
{
    var routes = await _unitOfWork.IcRoutes.Query().Where(r => r.ImportLineId == il.Id && !r.IsDeleted).ToListAsync();
    var dto = new IcImportLineWithRoutesDto
    {
        ImportLine = _mapper.Map<IcImportLineDto>(il),
        Routes = _mapper.Map<List<IcRouteDto>>(routes)
    };
    items.Add(dto);
}

var importLineDtos = items.Select(i => i.ImportLine).ToList();
var enriched = await _erpService.PopulateStockNamesAsync(importLineDtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>.ErrorResult(
        enriched.Message,
        enriched.ExceptionMessage,
        enriched.StatusCode
    );
}
var enrichedList = enriched.Data?.ToList() ?? importLineDtos;
for (int i = 0; i < items.Count && i < enrichedList.Count; i++)
{
    items[i].ImportLine = enrichedList[i];
}

return ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>.SuccessResult(items, _localizationService.GetLocalizedString("IcImportLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IcImportLineDto>> CreateAsync(CreateIcImportLineDto createDto)
        {
var entity = _mapper.Map<IcImportLine>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.IcImportLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<IcImportLineDto>(entity);
return ApiResponse<IcImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcImportLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<IcImportLineDto>> UpdateAsync(long id, UpdateIcImportLineDto updateDto)
        {
var entity = await _unitOfWork.IcImportLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<IcImportLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.IcImportLines.Update(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<IcImportLineDto>(entity);
return ApiResponse<IcImportLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcImportLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
var entity = await _unitOfWork.IcImportLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcImportLineNotFound"), _localizationService.GetLocalizedString("IcImportLineNotFound"), 404);
}
var routes = await _unitOfWork.IcRoutes.Query().Where(x => x.ImportLineId == id).ToListAsync();
if (routes.Any())
{
    var msg = _localizationService.GetLocalizedString("IcImportLineRoutesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    await _unitOfWork.IcImportLines.SoftDelete(id);
    await _unitOfWork.SaveChangesAsync();
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcImportLineDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}

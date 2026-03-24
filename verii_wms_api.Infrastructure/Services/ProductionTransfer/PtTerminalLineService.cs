using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class PtTerminalLineService : IPtTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PtTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetAllAsync()
        {
var entities = await _unitOfWork.PtTerminalLines.Query().ToListAsync();
var dtos = _mapper.Map<IEnumerable<PtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<PtTerminalLineDto>>> GetPagedAsync(PagedRequest request)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<PtTerminalLineDto>>.ErrorResult(
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

var result = new PagedResponse<PtTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<PtTerminalLineDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<PtTerminalLineDto>> GetByIdAsync(long id)
        {
var entity = await _unitOfWork.PtTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtTerminalLineNotFound"), _localizationService.GetLocalizedString("PtTerminalLineNotFound"), 404);
}
var dto = _mapper.Map<PtTerminalLineDto>(entity);
return ApiResponse<PtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
var entities = await _unitOfWork.PtTerminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
var entities = await _unitOfWork.PtTerminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<PtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<PtTerminalLineDto>> CreateAsync(CreatePtTerminalLineDto createDto)
        {
var entity = _mapper.Map<PtTerminalLine>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.PtTerminalLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<PtTerminalLineDto>(entity);
return ApiResponse<PtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtTerminalLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<PtTerminalLineDto>> UpdateAsync(long id, UpdatePtTerminalLineDto updateDto)
        {
var entity = await _unitOfWork.PtTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("PtTerminalLineNotFound"), _localizationService.GetLocalizedString("PtTerminalLineNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.PtTerminalLines.Update(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<PtTerminalLineDto>(entity);
return ApiResponse<PtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtTerminalLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
var exists = await _unitOfWork.PtTerminalLines.ExistsAsync(id);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtTerminalLineNotFound"), _localizationService.GetLocalizedString("PtTerminalLineNotFound"), 404);
}
await _unitOfWork.PtTerminalLines.SoftDelete(id);
await _unitOfWork.SaveChangesAsync();
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtTerminalLineDeletedSuccessfully"));
        }
    }
}

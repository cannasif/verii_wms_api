using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class MobileUserGroupMatchService : IMobileUserGroupMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public MobileUserGroupMatchService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetAllAsync()
        {
var entities = await _unitOfWork.MobileUserGroupMatches.GetAllAsync();
var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<MobileUserGroupMatchDto>>> GetPagedAsync(PagedRequest request)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.MobileUserGroupMatches.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync();
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync();

var dtos = _mapper.Map<List<MobileUserGroupMatchDto>>(entities);
var result = new PagedResponse<MobileUserGroupMatchDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<MobileUserGroupMatchDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<MobileUserGroupMatchDto>> GetByIdAsync(long id)
        {
var entity = await _unitOfWork.MobileUserGroupMatches.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null)
{
    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
    return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, message, 404);
}

var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByUserIdAsync(int userId)
        {
var entities = await _unitOfWork.MobileUserGroupMatches.Query().Where(x => x.UserId == userId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByGroupCodeAsync(string groupCode)
        {
var entities = await _unitOfWork.MobileUserGroupMatches.Query().Where(x => x.GroupCode == groupCode).ToListAsync();
var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<MobileUserGroupMatchDto>> CreateAsync(CreateMobileUserGroupMatchDto createDto)
        {
var entity = _mapper.Map<MobileUserGroupMatch>(createDto);
entity.CreatedDate = DateTimeProvider.Now;

await _unitOfWork.MobileUserGroupMatches.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();

var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchCreatedSuccessfully"));
        }

        public async Task<ApiResponse<MobileUserGroupMatchDto>> UpdateAsync(long id, UpdateMobileUserGroupMatchDto updateDto)
        {
var entity = await _unitOfWork.MobileUserGroupMatches.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null)
{
    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
    return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, message, 404);
}

_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;

_unitOfWork.MobileUserGroupMatches.Update(entity);
await _unitOfWork.SaveChangesAsync();

var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
var exists = await _unitOfWork.MobileUserGroupMatches.ExistsAsync(id);
if (!exists)
{
    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
    return ApiResponse<bool>.ErrorResult(message, message, 404);
}

await _unitOfWork.MobileUserGroupMatches.SoftDelete(id);
await _unitOfWork.SaveChangesAsync();

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("MobileUserGroupMatchDeletedSuccessfully"));
        }
    }
}

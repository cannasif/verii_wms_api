using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class PrHeaderSerialService : IPrHeaderSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrHeaderSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetAllAsync()
        {
var items = await _unitOfWork.PrHeaderSerials.Query().ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrHeaderSerialDto>>(items);
return ApiResponse<IEnumerable<PrHeaderSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<PrHeaderSerialDto>>> GetPagedAsync(PagedRequest request)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<PrHeaderSerialDto>>.ErrorResult(
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

var result = new PagedResponse<PrHeaderSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<PrHeaderSerialDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<PrHeaderSerialDto>> GetByIdAsync(long id)
        {
var entity = await _unitOfWork.PrHeaderSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
}
var dto = _mapper.Map<PrHeaderSerialDto>(entity);
return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetByHeaderIdAsync(long headerId)
        {
var items = await _unitOfWork.PrHeaderSerials.Query().Where(x => x.HeaderId == headerId).ToListAsync();
var dtos = _mapper.Map<IEnumerable<PrHeaderSerialDto>>(items);
return ApiResponse<IEnumerable<PrHeaderSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PrHeaderSerialRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PrHeaderSerialDto>> CreateAsync(CreatePrHeaderSerialDto createDto)
        {
var headerExists = await _unitOfWork.PrHeaders.ExistsAsync(createDto.HeaderId);
if (!headerExists)
{
    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderNotFound"), _localizationService.GetLocalizedString("PrHeaderNotFound"), 400);
}
var entity = _mapper.Map<PrHeaderSerial>(createDto);
entity.CreatedDate = DateTime.Now;
entity.IsDeleted = false;
await _unitOfWork.PrHeaderSerials.AddAsync(entity);
await _unitOfWork.SaveChangesAsync();
var dto = _mapper.Map<PrHeaderSerialDto>(entity);
return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialCreatedSuccessfully"));
        }

        public async Task<ApiResponse<PrHeaderSerialDto>> UpdateAsync(long id, UpdatePrHeaderSerialDto updateDto)
        {
var entity = await _unitOfWork.PrHeaderSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<PrHeaderSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
}

_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTime.Now;

_unitOfWork.PrHeaderSerials.Update(entity);
await _unitOfWork.SaveChangesAsync();

var dto = _mapper.Map<PrHeaderSerialDto>(entity);
return ApiResponse<PrHeaderSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PrHeaderSerialUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
var entity = await _unitOfWork.PrHeaderSerials.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
if (entity == null || entity.IsDeleted)
{
     return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PrHeaderSerialNotFound"), _localizationService.GetLocalizedString("PrHeaderSerialNotFound"), 404);
}

entity.IsDeleted = true;
entity.DeletedDate = DateTime.Now;

_unitOfWork.PrHeaderSerials.Update(entity);
await _unitOfWork.SaveChangesAsync();

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PrHeaderSerialDeletedSuccessfully"));
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitParameterService : ISitParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SitParameterService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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

        public async Task<ApiResponse<IEnumerable<SitParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitParameters.GetAllAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitParameterDto>>(entities);
return ApiResponse<IEnumerable<SitParameterDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SitParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.SitParameters.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<SitParameterDto>>(entities);
var result = new PagedResponse<SitParameterDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<SitParameterDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<SitParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitParameters.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    return ApiResponse<SitParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
}

var dto = _mapper.Map<SitParameterDto>(entity);
return ApiResponse<SitParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitParameterDto>> CreateAsync(CreateSitParameterDto createDto, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<SitParameter>(createDto);
await _unitOfWork.SitParameters.AddAsync(entity, requestCancellationToken);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<SitParameterDto>(entity);
return ApiResponse<SitParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SitParameterDto>> UpdateAsync(long id, UpdateSitParameterDto updateDto, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitParameters.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    return ApiResponse<SitParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
}

_mapper.Map(updateDto, entity);
_unitOfWork.SitParameters.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<SitParameterDto>(entity);
return ApiResponse<SitParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var exists = await _unitOfWork.SitParameters.ExistsAsync(id, requestCancellationToken);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
}

await _unitOfWork.SitParameters.SoftDelete(id, requestCancellationToken);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ParameterDeletedSuccessfully"));
        }
    }
}

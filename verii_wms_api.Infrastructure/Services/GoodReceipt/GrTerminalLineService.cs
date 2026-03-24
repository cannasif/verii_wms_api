using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class GrTerminalLineService : IGrTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GrTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.GrTerminalLines.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<GrTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<GrTerminalLineDto>>.ErrorResult(
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

var result = new PagedResponse<GrTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<GrTerminalLineDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<GrTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.GrTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
}
var dto = _mapper.Map<GrTerminalLineDto>(entity);
return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.GrTerminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.GrTerminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<GrTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrTerminalLineDto>> CreateAsync(CreateGrTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<GrTerminalLine>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.GrTerminalLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<GrTerminalLineDto>(entity);
return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<GrTerminalLineDto>> UpdateAsync(long id, UpdateGrTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.GrTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<GrTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.GrTerminalLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<GrTerminalLineDto>(entity);
return ApiResponse<GrTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrTerminalLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var exists = await _unitOfWork.GrTerminalLines.ExistsAsync(id);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrTerminalLineNotFound"), _localizationService.GetLocalizedString("GrTerminalLineNotFound"), 404);
}
await _unitOfWork.GrTerminalLines.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrTerminalLineDeletedSuccessfully"));
        }
    }
}

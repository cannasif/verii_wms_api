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
    public class WtTerminalLineService : IWtTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WtTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtTerminalLines
    .FindAsync(x => !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);  
return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var allResult = await GetAllAsync();
if (!allResult.Success || allResult.Data == null)
{
    return ApiResponse<PagedResponse<WtTerminalLineDto>>.ErrorResult(
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

var result = new PagedResponse<WtTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<WtTerminalLineDto>>.SuccessResult(result, allResult.Message);
        }


        public async Task<ApiResponse<WtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WtTerminalLines
    .Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);

if (entity == null || entity.IsDeleted)
{
    return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
}

var dto = _mapper.Map<WtTerminalLineDto>(entity);
return ApiResponse<WtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByTerminalIdAsync(long terminalId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtTerminalLines
    .FindAsync(x => x.TerminalUserId == terminalId && !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtTerminalLines
    .FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WtTerminalLines
    .FindAsync(x => x.TerminalUserId == userId && !x.IsDeleted);
var dtos = _mapper.Map<IEnumerable<WtTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<WtTerminalLineDto>> CreateAsync(CreateWtTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WtTerminalLine>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;

await _unitOfWork.WtTerminalLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<WtTerminalLineDto>(entity);
return ApiResponse<WtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtTerminalLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WtTerminalLineDto>> UpdateAsync(long id, UpdateWtTerminalLineDto dto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WtTerminalLines
    .Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);

if (entity == null || entity.IsDeleted)
{
    return ApiResponse<WtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
}

_mapper.Map(dto, entity);
entity.UpdatedDate = DateTime.Now;

_unitOfWork.WtTerminalLines
    .Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var updatedDto = _mapper.Map<WtTerminalLineDto>(entity);
return ApiResponse<WtTerminalLineDto>.SuccessResult(updatedDto, _localizationService.GetLocalizedString("WtTerminalLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var exists = await _unitOfWork.WtTerminalLines.ExistsAsync(id);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtTerminalLineNotFound"), _localizationService.GetLocalizedString("WtTerminalLineNotFound"), 404);
}

await _unitOfWork.WtTerminalLines.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtTerminalLineDeletedSuccessfully"));
        }
    }
}

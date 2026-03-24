using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoTerminalLineService : IWoTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WoTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoTerminalLines.GetAllAsync();
var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WoTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.WoTerminalLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<WoTerminalLineDto>>(entities);
var result = new PagedResponse<WoTerminalLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<WoTerminalLineDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<WoTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WoTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null) return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineNotFound"), _localizationService.GetLocalizedString("WoTerminalLineNotFound"), 404);
var dto = _mapper.Map<WoTerminalLineDto>(entity);
return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoTerminalLines.FindAsync(x => x.HeaderId == headerId);
var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WoTerminalLines.FindAsync(x => x.TerminalUserId == userId);
var dtos = _mapper.Map<IEnumerable<WoTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WoTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<WoTerminalLineDto>> CreateAsync(CreateWoTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WoTerminalLine>(createDto);
var created = await _unitOfWork.WoTerminalLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoTerminalLineDto>(created);
return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WoTerminalLineDto>> UpdateAsync(long id, UpdateWoTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existing = await _unitOfWork.WoTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existing == null) return ApiResponse<WoTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WoTerminalLineNotFound"), _localizationService.GetLocalizedString("WoTerminalLineNotFound"), 404);
var entity = _mapper.Map(updateDto, existing);
_unitOfWork.WoTerminalLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WoTerminalLineDto>(entity);
return ApiResponse<WoTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoTerminalLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
await _unitOfWork.WoTerminalLines.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoTerminalLineDeletedSuccessfully"));
        }
    }
}

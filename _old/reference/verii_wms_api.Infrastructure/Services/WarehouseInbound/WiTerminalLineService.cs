using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiTerminalLineService : IWiTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WiTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WiTerminalLines.GetAllAsync();
var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<WiTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.WiTerminalLines.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<WiTerminalLineDto>>(entities);
var result = new PagedResponse<WiTerminalLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<WiTerminalLineDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<WiTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.WiTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null) return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineNotFound"), _localizationService.GetLocalizedString("WiTerminalLineNotFound"), 404);
var dto = _mapper.Map<WiTerminalLineDto>(entity);
return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WiTerminalLines.FindAsync(x => x.HeaderId == headerId);
var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.WiTerminalLines.FindAsync(x => x.TerminalUserId == userId);
var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<WiTerminalLineDto>> CreateAsync(CreateWiTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<WiTerminalLine>(createDto);
var created = await _unitOfWork.WiTerminalLines.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WiTerminalLineDto>(created);
return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineCreatedSuccessfully"));
        }

        public async Task<ApiResponse<WiTerminalLineDto>> UpdateAsync(long id, UpdateWiTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var existing = await _unitOfWork.WiTerminalLines.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (existing == null) return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineNotFound"), _localizationService.GetLocalizedString("WiTerminalLineNotFound"), 404);
var entity = _mapper.Map(updateDto, existing);
_unitOfWork.WiTerminalLines.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<WiTerminalLineDto>(entity);
return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
await _unitOfWork.WiTerminalLines.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiTerminalLineDeletedSuccessfully"));
        }
    }
}

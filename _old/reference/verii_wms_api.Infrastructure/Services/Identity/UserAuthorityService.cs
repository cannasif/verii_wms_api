using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserAuthorityService : IUserAuthorityService
    {
        private static readonly HashSet<string> AllowedAuthorityTitles = new(StringComparer.OrdinalIgnoreCase)
        {
            "user",
            "admin"
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public UserAuthorityService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<UserAuthorityDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
var entities = await _unitOfWork.UserAuthorities.GetAllAsync();
entities = entities
    .Where(x => IsAllowedAuthorityTitle(x.Title))
    .ToList();
var dtos = _mapper.Map<IEnumerable<UserAuthorityDto>>(entities);
return ApiResponse<IEnumerable<UserAuthorityDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("UserAuthorityRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<UserAuthorityDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
request ??= new PagedRequest();
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.UserAuthorities.Query();
query = query.Where(x => IsAllowedAuthorityTitle(x.Title));
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(RequestCancellationToken);
var entities = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(RequestCancellationToken);

var dtos = _mapper.Map<List<UserAuthorityDto>>(entities);
var result = new PagedResponse<UserAuthorityDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<UserAuthorityDto>>.SuccessResult(
    result,
    _localizationService.GetLocalizedString("UserAuthorityRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<UserAuthorityDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
var entity = await _unitOfWork.UserAuthorities.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(RequestCancellationToken);
if (entity == null || !IsAllowedAuthorityTitle(entity.Title))
{
    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
}

var dto = _mapper.Map<UserAuthorityDto>(entity);
return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<UserAuthorityDto>> CreateAsync(CreateUserAuthorityDto createDto, CancellationToken cancellationToken = default)
        {
if (!IsAllowedAuthorityTitle(createDto.Title))
{
    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("OnlyUserAndAdminRolesAllowed"), _localizationService.GetLocalizedString("OnlyUserAndAdminRolesAllowed"), 400);
}

var entity = _mapper.Map<UserAuthority>(createDto);
entity.Title = NormalizeAuthorityTitle(createDto.Title);
await _unitOfWork.UserAuthorities.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

var dto = _mapper.Map<UserAuthorityDto>(entity);
return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityCreatedSuccessfully"));
        }

        public async Task<ApiResponse<UserAuthorityDto>> UpdateAsync(long id, UpdateUserAuthorityDto updateDto, CancellationToken cancellationToken = default)
        {
if (!IsAllowedAuthorityTitle(updateDto.Title))
{
    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("OnlyUserAndAdminRolesAllowed"), _localizationService.GetLocalizedString("OnlyUserAndAdminRolesAllowed"), 400);
}

var entity = await _unitOfWork.UserAuthorities.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(RequestCancellationToken);
if (entity == null)
{
    return ApiResponse<UserAuthorityDto>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
}

_mapper.Map(updateDto, entity);
entity.Title = NormalizeAuthorityTitle(updateDto.Title);
_unitOfWork.UserAuthorities.Update(entity);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

var dto = _mapper.Map<UserAuthorityDto>(entity);
return ApiResponse<UserAuthorityDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserAuthorityUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
var exists = await _unitOfWork.UserAuthorities.ExistsAsync(id);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("UserAuthorityNotFound"), _localizationService.GetLocalizedString("UserAuthorityNotFound"), 404);
}

var hasUsersWithRole = await _unitOfWork.Users.Query()
    .Where(u => u.RoleId == id)
            .AnyAsync(RequestCancellationToken);
if (hasUsersWithRole)
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("UserAuthorityCannotDeleteWhenUsersAssigned"),
        _localizationService.GetLocalizedString("UserAuthorityCannotDeleteWhenUsersAssigned"),
        400);
}

await _unitOfWork.UserAuthorities.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("UserAuthorityDeletedSuccessfully"));
        }

        private static bool IsAllowedAuthorityTitle(string? title)
        {
            return !string.IsNullOrWhiteSpace(title) && AllowedAuthorityTitles.Contains(title.Trim());
        }

        private static string NormalizeAuthorityTitle(string? title)
        {
            return title?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        
    }
}

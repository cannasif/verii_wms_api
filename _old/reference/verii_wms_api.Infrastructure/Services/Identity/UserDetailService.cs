using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserDetailService : IUserDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;
        private readonly IFileUploadService _fileUploadService;

        public UserDetailService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IFileUploadService fileUploadService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _requestCancellationAccessor = requestCancellationAccessor;
            _fileUploadService = fileUploadService;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<UserDetailDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
var entity = await _unitOfWork.UserDetails.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(RequestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<UserDetailDto>.ErrorResult(
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        404);
}

var dto = _mapper.Map<UserDetailDto>(entity);
return ApiResponse<UserDetailDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<UserDetailDto>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
var entity = await _unitOfWork.UserDetails.Query()
      // Add AsNoTracking to prevent tracking conflicts
    .Where(x => x.UserId == userId && !x.IsDeleted)
            .FirstOrDefaultAsync(RequestCancellationToken);

if (entity == null)
{
    return ApiResponse<UserDetailDto>.ErrorResult(
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        404);
}

var dto = _mapper.Map<UserDetailDto>(entity);
return ApiResponse<UserDetailDto>.SuccessResult(dto, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<UserDetailDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
var entities = await _unitOfWork.UserDetails.Query()
    .Where(x => !x.IsDeleted)
    .ToListAsync(RequestCancellationToken);

var dtos = _mapper.Map<IEnumerable<UserDetailDto>>(entities);
return ApiResponse<IEnumerable<UserDetailDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<UserDetailDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var query = _unitOfWork.UserDetails.Query()
    .Where(x => !x.IsDeleted);

query = query.ApplyFilters(request.Filters, request.FilterLogic);

bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(RequestCancellationToken);
var items = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(RequestCancellationToken);

var dtos = _mapper.Map<List<UserDetailDto>>(items);
var result = new PagedResponse<UserDetailDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<UserDetailDto>>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<UserDetailDto>> CreateAsync(CreateUserDetailDto dto, CancellationToken cancellationToken = default)
        {
// Check if user exists
var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
if (user == null || user.IsDeleted)
{
    return ApiResponse<UserDetailDto>.ErrorResult(
        _localizationService.GetLocalizedString("UserNotFound"),
        _localizationService.GetLocalizedString("UserNotFound"),
        404);
}

// Check if user detail already exists
var existingDetail = await _unitOfWork.UserDetails.Query()
    .Where(x => x.UserId == dto.UserId && !x.IsDeleted)
            .FirstOrDefaultAsync(RequestCancellationToken);

if (existingDetail != null)
{
    return ApiResponse<UserDetailDto>.ErrorResult(
        _localizationService.GetLocalizedString("UserDetailAlreadyExists"),
        _localizationService.GetLocalizedString("UserDetailAlreadyExists"),
        400);
}

var entity = _mapper.Map<UserDetail>(dto);
await _unitOfWork.UserDetails.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

var result = _mapper.Map<UserDetailDto>(entity);
return ApiResponse<UserDetailDto>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailCreatedSuccessfully"));
        }

        public async Task<ApiResponse<UserDetailDto>> UpdateAsync(long id, UpdateUserDetailDto dto, CancellationToken cancellationToken = default)
        {
var entity = await _unitOfWork.UserDetails.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(RequestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<UserDetailDto>.ErrorResult(
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        404);
}

// If profile picture exists in entity and is being changed or removed, delete old one
if (!string.IsNullOrEmpty(entity.ProfilePictureUrl) && 
    (string.IsNullOrEmpty(dto.ProfilePictureUrl) || entity.ProfilePictureUrl != dto.ProfilePictureUrl))
{
    // Delete old profile picture before updating
    await _fileUploadService.DeleteProfilePictureAsync(entity.ProfilePictureUrl);
}

_mapper.Map(dto, entity);
entity.UpdatedDate = DateTimeProvider.Now;

// Use Update which will handle tracking correctly
_unitOfWork.UserDetails.Update(entity);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

var result = _mapper.Map<UserDetailDto>(entity);
return ApiResponse<UserDetailDto>.SuccessResult(result, _localizationService.GetLocalizedString("UserDetailUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
var entity = await _unitOfWork.UserDetails.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(RequestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        _localizationService.GetLocalizedString("UserDetailNotFound"),
        404);
}

entity.IsDeleted = true;
entity.DeletedDate = DateTimeProvider.Now;
_unitOfWork.UserDetails.Update(entity);
await _unitOfWork.SaveChangesAsync(RequestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("UserDetailDeletedSuccessfully"));
        }
    }
}

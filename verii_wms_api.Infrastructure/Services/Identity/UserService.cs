using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Models.UserPermissions;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<UserDto>>> GetAllUsersAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var query = _unitOfWork.Users
                    .Query()
                    .Include(u => u.RoleNavigation)
                    .ApplySearch(
                        request.Search,
                        nameof(User.Username),
                        nameof(User.Email),
                        nameof(User.FirstName),
                        nameof(User.LastName),
                        "RoleNavigation.Title")
                    .ApplyFilters(request.Filters, request.FilterLogic);

                var desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? nameof(User.Id), desc);

                var totalCount = await query.CountAsync();
                var users = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var data = _mapper.Map<List<UserDto>>(users);
                var paged = new PagedResponse<UserDto>(data, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<UserDto>>.SuccessResult(
                    paged,
                    _localizationService.GetLocalizedString("DataRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<UserDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _unitOfWork.Users
                    .Query()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserNotFound"),
                        _localizationService.GetLocalizedString("UserNotFound"),
                        404);
                }

                return ApiResponse<UserDto>.SuccessResult(
                    _mapper.Map<UserDto>(user),
                    _localizationService.GetLocalizedString("DataRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        400);
                }

                var usernameExists = await _unitOfWork.Users.Query()
                    .Where(x => x.Username == dto.Username)
                            .AnyAsync();
                if (usernameExists)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserAlreadyExists"),
                        _localizationService.GetLocalizedString("UserAlreadyExists"),
                        400);
                }

                var emailExists = await _unitOfWork.Users.Query()
                    .Where(x => x.Email == dto.Email)
                            .AnyAsync();
                if (emailExists)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserAlreadyExists"),
                        _localizationService.GetLocalizedString("UserAlreadyExists"),
                        400);
                }

                var roleExists = await _unitOfWork.UserAuthorities.Query()
                    .Where(x => x.Id == dto.RoleId)
                            .AnyAsync();
                if (!roleExists)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        400);
                }

                if (dto.PermissionGroupIds != null)
                {
                    var validateGroups = await ValidatePermissionGroupIdsAsync(dto.PermissionGroupIds);
                    if (!validateGroups.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(validateGroups.Message, validateGroups.ExceptionMessage, validateGroups.StatusCode);
                    }

                    var validateSystemAdminAssignment = await ValidateSystemAdminGroupAssignmentAsync(dto.RoleId, dto.PermissionGroupIds);
                    if (!validateSystemAdminAssignment.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(
                            validateSystemAdminAssignment.Message,
                            validateSystemAdminAssignment.ExceptionMessage,
                            validateSystemAdminAssignment.StatusCode);
                    }
                }

                var plainPassword = string.IsNullOrWhiteSpace(dto.Password) ? GenerateTemporaryPassword() : dto.Password;

                var entity = _mapper.Map<User>(dto);
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                entity.IsEmailConfirmed = true;
                entity.IsActive = dto.IsActive ?? true;

                await _unitOfWork.Users.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                if (dto.PermissionGroupIds != null)
                {
                    var syncResult = await SyncUserPermissionGroupsAsync(entity.Id, dto.PermissionGroupIds);
                    if (!syncResult.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(syncResult.Message, syncResult.ExceptionMessage, syncResult.StatusCode);
                    }
                }

                var created = await _unitOfWork.Users.Query()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.Id == entity.Id)
                    .FirstOrDefaultAsync();

                BackgroundJob.Enqueue<IResetPasswordEmailJob>(job =>
                    job.SendUserCreatedEmailAsync(dto.Email, dto.Username, plainPassword, dto.FirstName, dto.LastName));

                return ApiResponse<UserDto>.SuccessResult(
                    _mapper.Map<UserDto>(created ?? entity),
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(long id, UpdateUserDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Users.Query(tracking: true)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _localizationService.GetLocalizedString("UserNotFound"),
                        _localizationService.GetLocalizedString("UserNotFound"),
                        404);
                }

                if (!string.IsNullOrWhiteSpace(dto.Email) && !dto.Email.Equals(entity.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await _unitOfWork.Users.Query()
                        .Where(x => x.Id != id && x.Email == dto.Email)
                            .AnyAsync();
                    if (emailExists)
                    {
                        return ApiResponse<UserDto>.ErrorResult(
                            _localizationService.GetLocalizedString("UserAlreadyExists"),
                            _localizationService.GetLocalizedString("UserAlreadyExists"),
                            400);
                    }
                }

                if (dto.RoleId.HasValue)
                {
                    var roleExists = await _unitOfWork.UserAuthorities.Query()
                        .Where(x => x.Id == dto.RoleId.Value)
                            .AnyAsync();
                    if (!roleExists)
                    {
                        return ApiResponse<UserDto>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            _localizationService.GetLocalizedString("ValidationError"),
                            400);
                    }
                }

                if (dto.PermissionGroupIds != null)
                {
                    var validateGroups = await ValidatePermissionGroupIdsAsync(dto.PermissionGroupIds);
                    if (!validateGroups.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(validateGroups.Message, validateGroups.ExceptionMessage, validateGroups.StatusCode);
                    }

                    var effectiveRoleId = dto.RoleId ?? entity.RoleId;
                    var validateSystemAdminAssignment = await ValidateSystemAdminGroupAssignmentAsync(effectiveRoleId, dto.PermissionGroupIds);
                    if (!validateSystemAdminAssignment.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(
                            validateSystemAdminAssignment.Message,
                            validateSystemAdminAssignment.ExceptionMessage,
                            validateSystemAdminAssignment.StatusCode);
                    }
                }

                _mapper.Map(dto, entity);
                _unitOfWork.Users.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                if (dto.PermissionGroupIds != null)
                {
                    var syncResult = await SyncUserPermissionGroupsAsync(entity.Id, dto.PermissionGroupIds);
                    if (!syncResult.Success)
                    {
                        return ApiResponse<UserDto>.ErrorResult(syncResult.Message, syncResult.ExceptionMessage, syncResult.StatusCode);
                    }
                }

                var updated = await _unitOfWork.Users.Query()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.Id == entity.Id)
                    .FirstOrDefaultAsync();

                return ApiResponse<UserDto>.SuccessResult(
                    _mapper.Map<UserDto>(updated ?? entity),
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        public async Task<ApiResponse<object>> DeleteUserAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.Users.Query().Where(x => x.Id == id)
                            .AnyAsync();
                if (!exists)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("UserNotFound"),
                        _localizationService.GetLocalizedString("UserNotFound"),
                        404);
                }

                await _unitOfWork.Users.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(new { }, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        private string GenerateTemporaryPassword()
        {
            var seed = Guid.NewGuid().ToString("N")[..10];
            return $"V3r!{seed}";
        }

        private async Task<ApiResponse<bool>> ValidatePermissionGroupIdsAsync(IEnumerable<long> permissionGroupIds)
        {
            try
            {
                var distinctGroupIds = permissionGroupIds.Distinct().ToList();
                if (distinctGroupIds.Count == 0)
                {
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
                }

                var validCount = await _unitOfWork.PermissionGroups.Query()
                    .CountAsync(x => distinctGroupIds.Contains(x.Id));

                if (validCount != distinctGroupIds.Count)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        400);
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        private async Task<ApiResponse<bool>> SyncUserPermissionGroupsAsync(long userId, IEnumerable<long> permissionGroupIds)
        {
            try
            {
                var distinctGroupIds = permissionGroupIds.Distinct().ToList();

                var currentLinks = await _unitOfWork.UserPermissionGroups.Query()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (var link in currentLinks.Where(x => !distinctGroupIds.Contains(x.PermissionGroupId)))
                {
                    await _unitOfWork.UserPermissionGroups.SoftDelete(link.Id);
                }

                var existingIds = currentLinks.Select(x => x.PermissionGroupId).ToHashSet();
                foreach (var groupId in distinctGroupIds.Where(id => !existingIds.Contains(id)))
                {
                    await _unitOfWork.UserPermissionGroups.AddAsync(new UserPermissionGroup
                    {
                        UserId = userId,
                        PermissionGroupId = groupId,
                        IsDeleted = false,
                        CreatedDate = DateTimeProvider.Now,
                    });
                }

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        private async Task<ApiResponse<bool>> ValidateSystemAdminGroupAssignmentAsync(long roleId, IEnumerable<long> permissionGroupIds)
        {
            try
            {
                var distinctGroupIds = permissionGroupIds.Distinct().ToList();
                if (distinctGroupIds.Count == 0)
                {
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
                }

                var hasSystemAdminGroup = await _unitOfWork.PermissionGroups.Query()
                    .Where(x => x.IsSystemAdmin && distinctGroupIds.Contains(x.Id))
                            .AnyAsync();

                if (!hasSystemAdminGroup)
                {
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
                }

                var isAdminRole = await IsAdminRoleAsync(roleId);
                if (!isAdminRole)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        "System Admin permission group can only be assigned to users with Admin role.",
                        400);
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    500);
            }
        }

        private async Task<bool> IsAdminRoleAsync(long roleId)
        {
            var roleTitle = await _unitOfWork.UserAuthorities.Query()
                .Where(x => x.Id == roleId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();

            return !string.IsNullOrWhiteSpace(roleTitle) &&
                   roleTitle.Contains("admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}

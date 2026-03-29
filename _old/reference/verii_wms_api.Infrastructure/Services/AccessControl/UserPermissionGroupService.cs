using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models.UserPermissions;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class UserPermissionGroupService : IUserPermissionGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public UserPermissionGroupService(IUnitOfWork unitOfWork, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        public async Task<ApiResponse<UserPermissionGroupDto>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var user = await _unitOfWork.Users.GetByIdAsync(userId, requestCancellationToken);
if (user == null)
{
    return ApiResponse<UserPermissionGroupDto>.ErrorResult(
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        StatusCodes.Status404NotFound);
}

var links = await _unitOfWork.UserPermissionGroups.Query()
    .Where(x => x.UserId == userId && !x.IsDeleted)
    .Include(x => x.PermissionGroup)
    .ToListAsync(requestCancellationToken);

var dto = new UserPermissionGroupDto
{
    UserId = userId,
    PermissionGroupIds = links.Select(x => x.PermissionGroupId).Distinct().OrderBy(x => x).ToList(),
    PermissionGroupNames = links
        .Where(x => x.PermissionGroup != null)
        .Select(x => x.PermissionGroup.Name)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(x => x)
        .ToList()
};

return ApiResponse<UserPermissionGroupDto>.SuccessResult(dto, _localizationService.GetLocalizedString("OperationSuccessful"));
        }

        public async Task<ApiResponse<UserPermissionGroupDto>> SetUserGroupsAsync(long userId, SetUserPermissionGroupsDto dto, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var user = await _unitOfWork.Users.GetByIdAsync(userId, requestCancellationToken);
if (user == null)
{
    return ApiResponse<UserPermissionGroupDto>.ErrorResult(
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        StatusCodes.Status404NotFound);
}

var distinctGroupIds = dto.PermissionGroupIds.Distinct().ToList();
if (distinctGroupIds.Count > 0)
{
    var validCount = await _unitOfWork.PermissionGroups.Query()
        .Where(x => !x.IsDeleted && distinctGroupIds.Contains(x.Id))
        .CountAsync(requestCancellationToken);

    if (validCount != distinctGroupIds.Count)
    {
        return ApiResponse<UserPermissionGroupDto>.ErrorResult(
            _localizationService.GetLocalizedString("ValidationError"),
            _localizationService.GetLocalizedString("ValidationError"),
            StatusCodes.Status400BadRequest);
    }

    var hasSystemAdminGroup = await _unitOfWork.PermissionGroups.Query()
        .Where(x => !x.IsDeleted && x.IsSystemAdmin && distinctGroupIds.Contains(x.Id))
        .AnyAsync(requestCancellationToken);

    if (hasSystemAdminGroup)
    {
        var isAdminRole = await IsAdminRoleAsync(user.RoleId, requestCancellationToken);
        if (!isAdminRole)
        {
            return ApiResponse<UserPermissionGroupDto>.ErrorResult(
                _localizationService.GetLocalizedString("ValidationError"),
                "System Admin permission group can only be assigned to users with Admin role.",
                StatusCodes.Status400BadRequest);
        }
    }
}

var currentLinks = await _unitOfWork.UserPermissionGroups.Query(ignoreQueryFilters: true)
    .Where(x => x.UserId == userId)
    .ToListAsync(requestCancellationToken);

foreach (var link in currentLinks.Where(x => !x.IsDeleted && !distinctGroupIds.Contains(x.PermissionGroupId)))
{
    await _unitOfWork.UserPermissionGroups.SoftDelete(link.Id, requestCancellationToken);
}

foreach (var groupId in distinctGroupIds)
{
    var existing = currentLinks.FirstOrDefault(x => x.PermissionGroupId == groupId);
    if (existing == null)
    {
        await _unitOfWork.UserPermissionGroups.AddAsync(new UserPermissionGroup
        {
            UserId = userId,
            PermissionGroupId = groupId
        }, requestCancellationToken);
        continue;
    }

    if (existing.IsDeleted)
    {
        existing.IsDeleted = false;
        existing.DeletedDate = null;
        existing.DeletedBy = null;
        _unitOfWork.UserPermissionGroups.Update(existing);
    }
}

await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return await GetByUserIdAsync(userId, requestCancellationToken);
        }

        private async Task<bool> IsAdminRoleAsync(long roleId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            var roleTitle = await _unitOfWork.UserAuthorities.Query()
                .Where(x => !x.IsDeleted && x.Id == roleId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync(requestCancellationToken);

            return !string.IsNullOrWhiteSpace(roleTitle) &&
                   roleTitle.Contains("admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}

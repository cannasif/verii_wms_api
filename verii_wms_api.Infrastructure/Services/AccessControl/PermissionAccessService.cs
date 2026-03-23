using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PermissionAccessService : IPermissionAccessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAccessService(
            IUnitOfWork unitOfWork,
            ILocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<MyPermissionsDto>> GetMyPermissionsAsync()
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                {
                    return ApiResponse<MyPermissionsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("Unauthorized"),
                        _localizationService.GetLocalizedString("Unauthorized"),
                        StatusCodes.Status401Unauthorized);
                }

                var user = await _unitOfWork.Users.Query()
                    .AsNoTracking()
                    .Include(x => x.RoleNavigation)
                    .Where(x => x.Id == userId && !x.IsDeleted)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return ApiResponse<MyPermissionsDto>.ErrorResult(
                        _localizationService.GetLocalizedString("AuthUserNotFound"),
                        _localizationService.GetLocalizedString("AuthUserNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var userGroupLinks = await _unitOfWork.UserPermissionGroups.Query()
                    .AsNoTracking()
                    .Where(x => x.UserId == userId && !x.IsDeleted)
                    .Include(x => x.PermissionGroup)
                    .ThenInclude(x => x.GroupPermissions.Where(gp => !gp.IsDeleted))
                    .ThenInclude(x => x.PermissionDefinition)
                    .ToListAsync();

                var roleTitle = user.RoleNavigation?.Title ?? "user";
                var isSystemAdmin = userGroupLinks.Any(x => x.PermissionGroup.IsSystemAdmin);

                if (!isSystemAdmin && userGroupLinks.Count == 0 &&
                    roleTitle.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    isSystemAdmin = true;
                }

                var permissionCodes = isSystemAdmin
                    ? new List<string>()
                    : userGroupLinks
                        .SelectMany(x => x.PermissionGroup.GroupPermissions)
                        .Where(x => !x.IsDeleted && x.PermissionDefinition != null && !x.PermissionDefinition.IsDeleted && x.PermissionDefinition.IsActive)
                        .Select(x => x.PermissionDefinition.Code)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(x => x)
                        .ToList();

                var response = new MyPermissionsDto
                {
                    UserId = userId,
                    RoleTitle = roleTitle,
                    IsSystemAdmin = isSystemAdmin,
                    PermissionGroups = userGroupLinks
                        .Select(x => x.PermissionGroup.Name)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(x => x)
                        .ToList(),
                    PermissionCodes = permissionCodes
                };

                return ApiResponse<MyPermissionsDto>.SuccessResult(
                    response,
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<MyPermissionsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetById"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}

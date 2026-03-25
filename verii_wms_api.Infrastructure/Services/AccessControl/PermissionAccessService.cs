using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PermissionAccessService : IPermissionAccessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public PermissionAccessService(
            IUnitOfWork unitOfWork,
            ILocalizationService localizationService,
            ICurrentUserService executionContextAccessor,
            IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _executionContextAccessor = executionContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        public async Task<ApiResponse<MyPermissionsDto>> GetMyPermissionsAsync(CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var userId = _executionContextAccessor.UserId;
if (!userId.HasValue)
{
    return ApiResponse<MyPermissionsDto>.ErrorResult(
        _localizationService.GetLocalizedString("Unauthorized"),
        _localizationService.GetLocalizedString("Unauthorized"),
        StatusCodes.Status401Unauthorized);
}

var user = await _unitOfWork.Users.Query()
    .Include(x => x.RoleNavigation)
    .Where(x => x.Id == userId.Value && !x.IsDeleted)
    .FirstOrDefaultAsync(requestCancellationToken);

if (user == null)
{
    return ApiResponse<MyPermissionsDto>.ErrorResult(
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        _localizationService.GetLocalizedString("AuthUserNotFound"),
        StatusCodes.Status404NotFound);
}

var userGroupLinks = await _unitOfWork.UserPermissionGroups.Query()
    .Where(x => x.UserId == userId.Value && !x.IsDeleted)
    .Include(x => x.PermissionGroup)
    .ThenInclude(x => x.GroupPermissions.Where(gp => !gp.IsDeleted))
    .ThenInclude(x => x.PermissionDefinition)
    .ToListAsync(requestCancellationToken);

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
    UserId = userId.Value,
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
    }
}

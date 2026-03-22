using Microsoft.AspNetCore.Authorization;

namespace WMS_WEBAPI.Security
{
    public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(string permissionCode)
        {
            Policy = PermissionPolicy.Build(permissionCode);
        }
    }
}

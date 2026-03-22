using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace WMS_WEBAPI.Security
{
    public sealed class PermissionAuthorizationConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    if (HasAllowAnonymous(controller, action) || !HasAuthorize(controller, action))
                    {
                        continue;
                    }

                    var permissionCode = PermissionRouteResolver.Resolve(controller.ControllerName, action);
                    if (string.IsNullOrWhiteSpace(permissionCode))
                    {
                        continue;
                    }

                    action.Filters.Add(new AuthorizeFilter(PermissionPolicy.Build(permissionCode)));
                }
            }
        }

        private static bool HasAllowAnonymous(ControllerModel controller, ActionModel action) =>
            controller.Attributes.OfType<AllowAnonymousAttribute>().Any() ||
            action.Attributes.OfType<AllowAnonymousAttribute>().Any();

        private static bool HasAuthorize(ControllerModel controller, ActionModel action) =>
            controller.Attributes.OfType<AuthorizeAttribute>().Any() ||
            action.Attributes.OfType<AuthorizeAttribute>().Any();
    }
}

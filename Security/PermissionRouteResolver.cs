using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WMS_WEBAPI.Security
{
    public static class PermissionRouteResolver
    {
        private static readonly IReadOnlyDictionary<string, string> ControllerPermissionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["PermissionDefinition"] = "access-control.permission-definitions",
            ["PermissionGroup"] = "access-control.permission-groups",
            ["UserPermissionGroup"] = "access-control.user-management",
            ["UserAuthority"] = "access-control.user-management",
            ["User"] = "access-control.user-management",
            ["SmtpSettings"] = "access-control.mail-settings",
            ["Mail"] = "access-control.mail-settings",
            ["Hangfire"] = "access-control.hangfire-monitoring",

            ["GrHeader"] = "wms.goods-receipt",
            ["GrLine"] = "wms.goods-receipt",
            ["GrImportDocument"] = "wms.goods-receipt",
            ["GrImportLine"] = "wms.goods-receipt",
            ["GrRoute"] = "wms.goods-receipt",
            ["GrTerminalLine"] = "wms.goods-receipt",
            ["GrLineSerial"] = "wms.goods-receipt",

            ["WtHeader"] = "wms.transfer",
            ["WtLine"] = "wms.transfer",
            ["WtImportLine"] = "wms.transfer",
            ["WtRoute"] = "wms.transfer",
            ["WtTerminalLine"] = "wms.transfer",
            ["WtLineSerial"] = "wms.transfer",

            ["SitHeader"] = "wms.subcontracting.issue",
            ["SitLine"] = "wms.subcontracting.issue",
            ["SitImportLine"] = "wms.subcontracting.issue",
            ["SitRoute"] = "wms.subcontracting.issue",
            ["SitTerminalLine"] = "wms.subcontracting.issue",
            ["SitLineSerial"] = "wms.subcontracting.issue",

            ["SrtHeader"] = "wms.subcontracting.receipt",
            ["SrtLine"] = "wms.subcontracting.receipt",
            ["SrtImportLine"] = "wms.subcontracting.receipt",
            ["SrtRoute"] = "wms.subcontracting.receipt",
            ["SrtTerminalLine"] = "wms.subcontracting.receipt",
            ["SrtLineSerial"] = "wms.subcontracting.receipt",

            ["WiHeader"] = "wms.warehouse.inbound",
            ["WiLine"] = "wms.warehouse.inbound",
            ["WiImportLine"] = "wms.warehouse.inbound",
            ["WiRoute"] = "wms.warehouse.inbound",
            ["WiTerminalLine"] = "wms.warehouse.inbound",
            ["WiLineSerial"] = "wms.warehouse.inbound",

            ["WoHeader"] = "wms.warehouse.outbound",
            ["WoLine"] = "wms.warehouse.outbound",
            ["WoImportLine"] = "wms.warehouse.outbound",
            ["WoRoute"] = "wms.warehouse.outbound",
            ["WoTerminalLine"] = "wms.warehouse.outbound",
            ["WoLineSerial"] = "wms.warehouse.outbound",

            ["ShHeader"] = "wms.shipment",
            ["ShLine"] = "wms.shipment",
            ["ShImportLine"] = "wms.shipment",
            ["ShRoute"] = "wms.shipment",
            ["ShTerminalLine"] = "wms.shipment",
            ["ShLineSerial"] = "wms.shipment",

            ["PHeader"] = "wms.package",
            ["PLine"] = "wms.package",
            ["PPackage"] = "wms.package",

            ["GrParameter"] = "wms.parameters.gr",
            ["WtParameter"] = "wms.parameters.wt",
            ["WoParameter"] = "wms.parameters.wo",
            ["WiParameter"] = "wms.parameters.wi",
            ["ShParameter"] = "wms.parameters.sh",
            ["SrtParameter"] = "wms.parameters.srt",
            ["SitParameter"] = "wms.parameters.sit",
            ["PtParameter"] = "wms.parameters.pt",
            ["PrParameter"] = "wms.parameters.pr",
            ["IcParameter"] = "wms.parameters.ic",
            ["PParameter"] = "wms.parameters.p",

            ["PrHeader"] = "wms.production",
            ["PrLine"] = "wms.production",
            ["PrImportLine"] = "wms.production",
            ["PrRoute"] = "wms.production",
            ["PrTerminalLine"] = "wms.production",
            ["PrLineSerial"] = "wms.production",
            ["PrHeaderSerial"] = "wms.production",

            ["PtHeader"] = "wms.production-transfer",
            ["PtLine"] = "wms.production-transfer",
            ["PtImportLine"] = "wms.production-transfer",
            ["PtRoute"] = "wms.production-transfer",
            ["PtTerminalLine"] = "wms.production-transfer",
            ["PtLineSerial"] = "wms.production-transfer",

            ["ICHeader"] = "wms.inventory-count",
            ["IcImportLine"] = "wms.inventory-count",
            ["IcRoute"] = "wms.inventory-count",
            ["IcTerminalLine"] = "wms.inventory-count",
        };

        private static readonly HashSet<string> SkippedControllers = new(StringComparer.OrdinalIgnoreCase)
        {
            "Auth",
            "Erp",
            "Notification",
            "UserDetail",
            "CustomerMirror",
            "StockMirror",
            "GoodReciptFunctions",
            "WiFunction",
            "WoFunction",
            "WtFunction",
            "ShFunction",
            "SitFunction",
            "SrtFunction",
            "PrFunction",
            "PtFunction",
            "MobilePageGroup",
            "MobileUserGroupMatch",
            "MobilemenuHeader",
            "MobilemenuLine",
            "PlatformPageGroup",
            "PlatformUserGroupMatch",
            "SidebarmenuHeader",
            "SidebarmenuLine",
            "Job"
        };

        public static string? Resolve(string controllerName, ActionModel action)
        {
            if (SkippedControllers.Contains(controllerName))
            {
                return null;
            }

            if (!ControllerPermissionMap.TryGetValue(controllerName, out var permissionBase))
            {
                return null;
            }

            var httpMethods = action.Selectors
                .SelectMany(selector => selector.ActionConstraints?.OfType<HttpMethodActionConstraint>() ?? Enumerable.Empty<HttpMethodActionConstraint>())
                .SelectMany(constraint => constraint.HttpMethods)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var method = httpMethods.FirstOrDefault()?.ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(method))
            {
                return null;
            }

            var operation = method switch
            {
                "GET" => "view",
                "HEAD" => "view",
                "POST" => "create",
                "PUT" => "update",
                "PATCH" => "update",
                "DELETE" => "update",
                _ => null
            };

            if (operation == null)
            {
                return null;
            }

            return $"{permissionBase}.{operation}";
        }
    }
}

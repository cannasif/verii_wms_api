namespace WMS_WEBAPI.Security
{
    public static class PermissionPolicy
    {
        private const string Prefix = "Permission:";

        public static string Build(string permissionCode) => $"{Prefix}{permissionCode}";
    }
}

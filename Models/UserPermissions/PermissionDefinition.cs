using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models.UserPermissions
{
    public class PermissionDefinition : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<PermissionGroupPermission> GroupPermissions { get; set; } = new List<PermissionGroupPermission>();
    }
}

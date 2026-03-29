using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models.UserPermissions
{
    public class UserPermissionGroup : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; } = null!;
    }
}

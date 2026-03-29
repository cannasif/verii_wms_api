using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.AccessControl;

public sealed class PermissionDefinition : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PermissionGroupPermission> GroupPermissions { get; set; } = new List<PermissionGroupPermission>();
}

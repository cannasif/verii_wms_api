using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrHeaderAssignment : BaseEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public string AssignmentType { get; set; } = "Primary";
    public DateTime AssignedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

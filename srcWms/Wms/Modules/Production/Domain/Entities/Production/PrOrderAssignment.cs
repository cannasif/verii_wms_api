using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrOrderAssignment : BaseEntity
{
    public long OrderId { get; set; }
    public PrOrder Order { get; set; } = null!;
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public string AssignmentType { get; set; } = "Primary";
    public DateTime AssignedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Note { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrOperation : BaseEntity
{
    public long OrderId { get; set; }
    public PrOrder Order { get; set; } = null!;
    public string? OperationNo { get; set; }
    public string OperationType { get; set; } = "ProductionRun";
    public string Status { get; set; } = "Open";
    public long? WorkcenterId { get; set; }
    public long? MachineId { get; set; }
    public long? ExecutedByUserId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? PlannedDurationMinutes { get; set; }
    public int? ActualDurationMinutes { get; set; }
    public int? PauseDurationMinutes { get; set; }
    public int? NetWorkingDurationMinutes { get; set; }
    public string? Description { get; set; }

    public ICollection<PrOperationLine> Lines { get; set; } = new List<PrOperationLine>();
    public ICollection<PrOperationEvent> Events { get; set; } = new List<PrOperationEvent>();
}

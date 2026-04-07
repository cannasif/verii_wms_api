using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrOperationEvent : BaseEntity
{
    public long OperationId { get; set; }
    public PrOperation Operation { get; set; } = null!;
    public long OrderId { get; set; }
    public PrOrder Order { get; set; } = null!;
    public string EventType { get; set; } = "Start";
    public string? EventReasonCode { get; set; }
    public string? EventNote { get; set; }
    public DateTime EventAt { get; set; }
    public int? DurationMinutes { get; set; }
    public long? PerformedByUserId { get; set; }
    public long? WorkcenterId { get; set; }
    public long? MachineId { get; set; }
}

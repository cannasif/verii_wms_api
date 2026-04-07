using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrOrderDependency : BaseEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public long PredecessorOrderId { get; set; }
    public PrOrder PredecessorOrder { get; set; } = null!;
    public long SuccessorOrderId { get; set; }
    public PrOrder SuccessorOrder { get; set; } = null!;
    public string DependencyType { get; set; } = "FinishToStart";
    public bool IsRequired { get; set; } = true;
    public int LagMinutes { get; set; }
    public bool RequiredTransferCompleted { get; set; }
    public bool RequiredOutputAvailable { get; set; }
    public string? ConditionNote { get; set; }
}

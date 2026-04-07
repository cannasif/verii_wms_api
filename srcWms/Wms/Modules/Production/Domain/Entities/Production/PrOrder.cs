using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Domain.Entities.Production;

public sealed class PrOrder : BaseEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public string OrderNo { get; set; } = null!;
    public DateTime? OrderDate { get; set; }
    public string? Description { get; set; }
    public string OrderType { get; set; } = "Production";
    public string Status { get; set; } = "Draft";
    public int Priority { get; set; }
    public int? SequenceNo { get; set; }
    public int? ParallelGroupNo { get; set; }
    public long ProducedStockId { get; set; }
    public string ProducedStockCode { get; set; } = null!;
    public long? ProducedYapKodId { get; set; }
    public string? ProducedYapKod { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? StartedQuantity { get; set; }
    public decimal? CompletedQuantity { get; set; }
    public decimal? ScrapQuantity { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public long? TargetWarehouseId { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public bool CanStartManually { get; set; }
    public bool AutoStartWhenDependenciesDone { get; set; }
    public bool IsTransferRequiredBeforeStart { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string? BlockedReason { get; set; }
    public string? StatusNote { get; set; }

    public ICollection<PrOrderDependency> DependenciesAsPredecessor { get; set; } = new List<PrOrderDependency>();
    public ICollection<PrOrderDependency> DependenciesAsSuccessor { get; set; } = new List<PrOrderDependency>();
    public ICollection<PrOrderAssignment> Assignments { get; set; } = new List<PrOrderAssignment>();
    public ICollection<PrOrderOutput> Outputs { get; set; } = new List<PrOrderOutput>();
    public ICollection<PrOrderConsumption> Consumptions { get; set; } = new List<PrOrderConsumption>();
    public ICollection<PrOperation> Operations { get; set; } = new List<PrOperation>();
    public ICollection<PtHeader> ProductionTransfers { get; set; } = new List<PtHeader>();
}

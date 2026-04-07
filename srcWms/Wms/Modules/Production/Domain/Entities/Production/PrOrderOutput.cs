using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrOrderOutput : BaseEntity
{
    public long OrderId { get; set; }
    public PrOrder Order { get; set; } = null!;
    public int? SequenceNo { get; set; }
    public long StockId { get; set; }
    public string StockCode { get; set; } = null!;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public string? Unit { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? ProducedQuantity { get; set; }
    public string TrackingMode { get; set; } = "None";
    public string SerialEntryMode { get; set; } = "Optional";
    public long? TargetWarehouseId { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string Status { get; set; } = "Draft";

    public ICollection<PrOperationLine> OperationLines { get; set; } = new List<PrOperationLine>();
}

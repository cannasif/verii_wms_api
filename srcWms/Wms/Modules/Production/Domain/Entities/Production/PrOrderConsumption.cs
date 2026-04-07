using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Domain.Entities.Production;

public sealed class PrOrderConsumption : BaseEntity
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
    public decimal? ConsumedQuantity { get; set; }
    public string TrackingMode { get; set; } = "None";
    public string SerialEntryMode { get; set; } = "Optional";
    public long? SourceWarehouseId { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public bool IsBackflush { get; set; }
    public bool IsMandatory { get; set; } = true;
    public string Status { get; set; } = "Draft";

    public ICollection<PrOperationLine> OperationLines { get; set; } = new List<PrOperationLine>();
    public ICollection<PtLine> ProductionTransferLines { get; set; } = new List<PtLine>();
}

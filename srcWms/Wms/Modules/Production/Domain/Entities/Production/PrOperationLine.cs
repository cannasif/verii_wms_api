using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Domain.Entities.Production;

public sealed class PrOperationLine : BaseEntity
{
    public long OperationId { get; set; }
    public PrOperation Operation { get; set; } = null!;
    public long OrderId { get; set; }
    public PrOrder Order { get; set; } = null!;
    public string LineRole { get; set; } = "Consumption";
    public long? OrderOutputId { get; set; }
    public PrOrderOutput? OrderOutput { get; set; }
    public long? OrderConsumptionId { get; set; }
    public PrOrderConsumption? OrderConsumption { get; set; }
    public long StockId { get; set; }
    public string StockCode { get; set; } = null!;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? SerialNo1 { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? LotNo { get; set; }
    public string? BatchNo { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public long? TargetWarehouseId { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string? ScannedBarcode { get; set; }

    public ICollection<PtLineSerial> ProductionTransferSerialLines { get; set; } = new List<PtLineSerial>();
}

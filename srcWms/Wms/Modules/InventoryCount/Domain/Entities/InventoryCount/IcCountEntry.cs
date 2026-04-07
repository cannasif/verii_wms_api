using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcCountEntry : BaseEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public long LineId { get; set; }
    public IcLine Line { get; set; } = null!;
    public int EntryNo { get; set; }
    public string EntryType { get; set; } = "FirstCount";
    public decimal EnteredQuantity { get; set; }
    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }
    public DateTime EnteredAt { get; set; }
    public long? EnteredByUserId { get; set; }
    public string? DeviceCode { get; set; }
    public string? Note { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcScope : BaseEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public int? SequenceNo { get; set; }
    public string ScopeType { get; set; } = "Warehouse";
    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public long? StockId { get; set; }
    public string? StockCode { get; set; }
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<IcLine> Lines { get; set; } = new List<IcLine>();
}

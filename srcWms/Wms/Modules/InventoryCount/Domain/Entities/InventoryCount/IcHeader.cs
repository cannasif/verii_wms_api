using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcHeader : BaseHeaderEntity
{
    public string? WarehouseCode { get; set; }
    public long? WarehouseId { get; set; }
    public string? ProductCode { get; set; }
    public string? CellCode { get; set; }
    public byte Type { get; set; }
    public ICollection<IcTerminalLine> TerminalLines { get; set; } = new List<IcTerminalLine>();
    public ICollection<IcImportLine> ImportLines { get; set; } = new List<IcImportLine>();
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public WiHeader Header { get; set; } = null!;

    public ICollection<WiImportLine> ImportLines { get; set; } = new List<WiImportLine>();
    public ICollection<WiLineSerial> SerialLines { get; set; } = new List<WiLineSerial>();
}

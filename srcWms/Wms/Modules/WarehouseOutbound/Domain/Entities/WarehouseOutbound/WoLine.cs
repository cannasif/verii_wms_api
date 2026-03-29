using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public WoHeader Header { get; set; } = null!;

    public ICollection<WoImportLine> ImportLines { get; set; } = new List<WoImportLine>();
    public ICollection<WoLineSerial> SerialLines { get; set; } = new List<WoLineSerial>();
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public ShHeader Header { get; set; } = null!;

    public ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
    public ICollection<ShLineSerial> SerialLines { get; set; } = new List<ShLineSerial>();
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public WtHeader Header { get; set; } = null!;

    public ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
    public ICollection<WtLineSerial> SerialLines { get; set; } = new List<WtLineSerial>();
}

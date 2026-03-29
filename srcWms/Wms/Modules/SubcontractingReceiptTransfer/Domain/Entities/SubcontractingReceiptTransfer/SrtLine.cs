using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public SrtHeader Header { get; set; } = null!;
    public ICollection<SrtImportLine> ImportLines { get; set; } = new List<SrtImportLine>();
    public ICollection<SrtLineSerial> SerialLines { get; set; } = new List<SrtLineSerial>();
}

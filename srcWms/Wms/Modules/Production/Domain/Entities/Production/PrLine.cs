using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
    public ICollection<PrLineSerial> SerialLines { get; set; } = new List<PrLineSerial>();
}

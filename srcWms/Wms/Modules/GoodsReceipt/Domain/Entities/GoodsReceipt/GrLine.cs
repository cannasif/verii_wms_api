using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public GrHeader Header { get; set; } = null!;
    public ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
    public ICollection<GrLineSerial> SerialLines { get; set; } = new List<GrLineSerial>();
}

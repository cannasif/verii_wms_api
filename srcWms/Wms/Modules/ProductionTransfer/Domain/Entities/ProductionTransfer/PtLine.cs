using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public PtHeader Header { get; set; } = null!;
    public ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
    public ICollection<PtLineSerial> SerialLines { get; set; } = new List<PtLineSerial>();
}

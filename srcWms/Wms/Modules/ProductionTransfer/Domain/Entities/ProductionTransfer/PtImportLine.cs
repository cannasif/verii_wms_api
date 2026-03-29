using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public PtHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public PtLine? Line { get; set; }
    public ICollection<PtRoute> Routes { get; set; } = new List<PtRoute>();
}

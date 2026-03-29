using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public PrLine? Line { get; set; }
    public ICollection<PrRoute> Routes { get; set; } = new List<PrRoute>();
}

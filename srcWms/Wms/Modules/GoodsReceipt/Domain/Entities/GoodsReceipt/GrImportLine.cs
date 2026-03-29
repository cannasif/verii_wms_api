using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public GrHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public GrLine? Line { get; set; }
    public ICollection<GrRoute> Routes { get; set; } = new List<GrRoute>();
}

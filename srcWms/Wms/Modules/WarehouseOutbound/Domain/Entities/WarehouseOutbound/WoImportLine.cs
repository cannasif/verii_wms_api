using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public WoHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public WoLine? Line { get; set; }

    public ICollection<WoRoute> Routes { get; set; } = new List<WoRoute>();
}

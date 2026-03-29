using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public ShHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public ShLine? Line { get; set; }

    public ICollection<ShRoute> Routes { get; set; } = new List<ShRoute>();
}

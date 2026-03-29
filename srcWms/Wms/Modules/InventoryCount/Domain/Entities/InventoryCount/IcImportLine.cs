using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public ICollection<IcRoute> Routes { get; set; } = new List<IcRoute>();
}

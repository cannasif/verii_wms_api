using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public WtHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public WtLine? Line { get; set; }

    public ICollection<WtRoute> Routes { get; set; } = new List<WtRoute>();
}

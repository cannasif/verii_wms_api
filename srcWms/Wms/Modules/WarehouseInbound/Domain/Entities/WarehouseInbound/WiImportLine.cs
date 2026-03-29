using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public WiHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public WiLine? Line { get; set; }
}

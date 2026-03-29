using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public WiLine Line { get; set; } = null!;
}

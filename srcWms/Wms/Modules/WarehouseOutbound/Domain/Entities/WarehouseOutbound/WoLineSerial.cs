using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public WoLine Line { get; set; } = null!;
}

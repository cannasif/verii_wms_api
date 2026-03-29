using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public WtLine Line { get; set; } = null!;
}

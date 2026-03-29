using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public ShLine Line { get; set; } = null!;
}

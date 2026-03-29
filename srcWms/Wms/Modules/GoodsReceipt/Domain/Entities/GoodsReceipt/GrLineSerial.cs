using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrLineSerial : BaseLineSerialEntity
{
    public long? LineId { get; set; }
    public GrLine? Line { get; set; }
}

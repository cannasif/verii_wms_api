using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public PrLine Line { get; set; } = null!;
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public PtLine Line { get; set; } = null!;
}

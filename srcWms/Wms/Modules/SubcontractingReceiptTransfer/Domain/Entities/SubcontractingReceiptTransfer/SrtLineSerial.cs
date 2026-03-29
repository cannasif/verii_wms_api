using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public SrtLine Line { get; set; } = null!;
}

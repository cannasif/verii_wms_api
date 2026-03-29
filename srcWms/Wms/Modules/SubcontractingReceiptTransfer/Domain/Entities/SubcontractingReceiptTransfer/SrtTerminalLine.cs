using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public SrtHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

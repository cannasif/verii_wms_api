using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public PtHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

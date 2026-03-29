using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public WiHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public WoHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

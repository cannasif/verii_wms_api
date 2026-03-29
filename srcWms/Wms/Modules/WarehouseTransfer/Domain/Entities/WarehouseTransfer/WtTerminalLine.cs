using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public WtHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

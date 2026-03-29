using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public ShHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public PrHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

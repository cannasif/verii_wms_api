using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Identity;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public GrHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
    public User User { get; set; } = null!;
}

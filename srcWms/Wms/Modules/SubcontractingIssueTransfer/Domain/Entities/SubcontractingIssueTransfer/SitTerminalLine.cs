using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitTerminalLine : BaseEntity
{
    public long HeaderId { get; set; }
    public SitHeader Header { get; set; } = null!;
    public long TerminalUserId { get; set; }
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitLineSerial : BaseLineSerialEntity
{
    public long LineId { get; set; }
    public SitLine Line { get; set; } = null!;
}

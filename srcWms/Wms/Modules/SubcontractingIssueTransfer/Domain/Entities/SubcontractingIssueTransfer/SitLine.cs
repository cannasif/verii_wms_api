using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public SitHeader Header { get; set; } = null!;
    public ICollection<SitImportLine> ImportLines { get; set; } = new List<SitImportLine>();
    public ICollection<SitLineSerial> SerialLines { get; set; } = new List<SitLineSerial>();
}

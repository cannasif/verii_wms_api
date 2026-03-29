using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public SitHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public SitLine? Line { get; set; }
    public ICollection<SitRoute> Routes { get; set; } = new List<SitRoute>();
}

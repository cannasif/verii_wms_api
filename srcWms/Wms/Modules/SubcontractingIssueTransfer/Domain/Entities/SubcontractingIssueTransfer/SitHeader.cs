using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? TargetWarehouse { get; set; }
    public byte Type { get; set; }
    public ICollection<SitLine> Lines { get; set; } = new List<SitLine>();
    public ICollection<SitImportLine> ImportLines { get; set; } = new List<SitImportLine>();
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingIssueTransfer;

public sealed class SitRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public SitImportLine ImportLine { get; set; } = null!;
}

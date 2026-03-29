using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public GrImportLine ImportLine { get; set; } = null!;
}

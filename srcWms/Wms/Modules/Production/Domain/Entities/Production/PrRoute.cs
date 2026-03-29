using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public PrImportLine ImportLine { get; set; } = null!;
}

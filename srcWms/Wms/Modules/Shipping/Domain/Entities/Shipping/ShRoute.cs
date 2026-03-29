using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public ShImportLine ImportLine { get; set; } = null!;
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public WoImportLine ImportLine { get; set; } = null!;
}

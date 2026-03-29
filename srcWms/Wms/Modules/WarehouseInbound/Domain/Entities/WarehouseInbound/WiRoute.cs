using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public WiImportLine ImportLine { get; set; } = null!;
}

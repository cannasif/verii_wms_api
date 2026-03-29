using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public WtImportLine ImportLine { get; set; } = null!;
}

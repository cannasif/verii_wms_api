using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public PtImportLine ImportLine { get; set; } = null!;
}

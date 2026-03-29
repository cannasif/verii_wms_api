using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public SrtImportLine ImportLine { get; set; } = null!;
}

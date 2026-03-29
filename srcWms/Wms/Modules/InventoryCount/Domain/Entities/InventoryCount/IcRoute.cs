using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcRoute : BaseRouteEntity
{
    public long ImportLineId { get; set; }
    public IcImportLine ImportLine { get; set; } = null!;
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    public string? Barcode { get; set; }
    public int? OldWarehouse { get; set; }
    public int? NewWarehouse { get; set; }
}

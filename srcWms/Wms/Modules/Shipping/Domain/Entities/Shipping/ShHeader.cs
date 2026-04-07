using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Shipping;

public sealed class ShHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public string? SourceWarehouse { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? TargetWarehouse { get; set; }
    public long? TargetWarehouseId { get; set; }
    public long? ShipmentId { get; set; }

    public ICollection<ShLine> Lines { get; set; } = new List<ShLine>();
    public ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
}

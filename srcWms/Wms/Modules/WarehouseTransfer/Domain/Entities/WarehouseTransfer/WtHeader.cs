using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseTransfer;

public sealed class WtHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public string? SourceWarehouse { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? TargetWarehouse { get; set; }
    public long? TargetWarehouseId { get; set; }
    public bool ElectronicWaybill { get; set; }
    public long? ShipmentId { get; set; }

    public ICollection<WtLine> Lines { get; set; } = new List<WtLine>();
    public ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
}

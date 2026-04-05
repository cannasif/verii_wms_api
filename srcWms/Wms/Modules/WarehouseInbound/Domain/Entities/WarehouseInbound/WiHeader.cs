using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseInbound;

public sealed class WiHeader : BaseHeaderEntity
{
    public string InboundType { get; set; } = null!;
    public string? AccountCode { get; set; }
    public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public string? SourceWarehouse { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? TargetWarehouse { get; set; }
    public long? TargetWarehouseId { get; set; }

    public ICollection<WiLine> Lines { get; set; } = new List<WiLine>();
    public ICollection<WiImportLine> ImportLines { get; set; } = new List<WiImportLine>();
}

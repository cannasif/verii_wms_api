using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.WarehouseOutbound;

public sealed class WoHeader : BaseHeaderEntity
{
    public string OutboundType { get; set; } = null!;
    public string? AccountCode { get; set; }
    public string? CustomerCode { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? TargetWarehouse { get; set; }
    public byte Type { get; set; }

    public ICollection<WoLine> Lines { get; set; } = new List<WoLine>();
    public ICollection<WoImportLine> ImportLines { get; set; } = new List<WoImportLine>();
}

using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? TargetWarehouse { get; set; }
    public byte Type { get; set; }
    public ICollection<SrtLine> Lines { get; set; } = new List<SrtLine>();
    public ICollection<SrtImportLine> ImportLines { get; set; } = new List<SrtImportLine>();
}

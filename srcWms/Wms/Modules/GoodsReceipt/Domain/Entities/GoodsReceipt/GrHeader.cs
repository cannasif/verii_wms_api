using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrHeader : BaseHeaderEntity
{
    public string CustomerCode { get; set; } = null!;
    public long? CustomerId { get; set; }
    public bool ElectronicWaybill { get; set; }
    public bool ReturnCode { get; set; }
    public bool OCRSource { get; set; }
    public string? Description3 { get; set; }
    public string? Description4 { get; set; }
    public string? Description5 { get; set; }
    public ICollection<GrLine> Lines { get; set; } = new List<GrLine>();
    public ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
}

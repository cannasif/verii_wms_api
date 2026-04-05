using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Package;

public sealed class PLine : BaseEntity
{
    public long PackingHeaderId { get; set; }
    public PHeader PackingHeader { get; set; } = null!;
    public long PackageId { get; set; }
    public PPackage Package { get; set; } = null!;
    public string? Barcode { get; set; }
    public string StockCode { get; set; } = null!;
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public long? SourceRouteId { get; set; }
}

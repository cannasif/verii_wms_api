using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Package.Enums;

namespace Wms.Domain.Entities.Package;

public sealed class PPackage : BaseEntity
{
    public long PackingHeaderId { get; set; }
    public PHeader PackingHeader { get; set; } = null!;
    public string PackageNo { get; set; } = null!;
    public string PackageType { get; set; } = PPackageType.Box;
    public string? Barcode { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Volume { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? TareWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public bool IsMixed { get; set; }
    public string Status { get; set; } = PPackageStatus.Open;
    public ICollection<PLine> Lines { get; set; } = new List<PLine>();
}

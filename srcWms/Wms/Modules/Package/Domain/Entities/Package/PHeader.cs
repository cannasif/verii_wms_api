using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Package.Enums;

namespace Wms.Domain.Entities.Package;

public sealed class PHeader : BaseEntity
{
    public string? WarehouseCode { get; set; }
    public string PackingNo { get; set; } = null!;
    public DateTime? PackingDate { get; set; }
    public string? SourceType { get; set; }
    public long? SourceHeaderId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerAddress { get; set; }
    public string Status { get; set; } = PHeaderStatus.Draft;
    public decimal? TotalPackageCount { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? TotalNetWeight { get; set; }
    public decimal? TotalGrossWeight { get; set; }
    public decimal? TotalVolume { get; set; }
    public long? CarrierId { get; set; }
    public string? CarrierServiceType { get; set; }
    public string? TrackingNo { get; set; }
    public bool IsMatched { get; set; }
    public ICollection<PPackage> Packages { get; set; } = new List<PPackage>();
}

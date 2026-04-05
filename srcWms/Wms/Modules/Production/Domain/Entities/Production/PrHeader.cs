using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public decimal? Quantity { get; set; }
    public string? SourceWarehouse { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? TargetWarehouse { get; set; }
    public long? TargetWarehouseId { get; set; }

    public ICollection<PrLine> Lines { get; set; } = new List<PrLine>();
    public ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
}

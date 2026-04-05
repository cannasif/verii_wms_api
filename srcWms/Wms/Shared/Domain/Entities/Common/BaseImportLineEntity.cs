namespace Wms.Domain.Entities.Common;

public abstract class BaseImportLineEntity : BaseEntity
{
    public string StockCode { get; set; } = null!;
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description { get; set; }
}

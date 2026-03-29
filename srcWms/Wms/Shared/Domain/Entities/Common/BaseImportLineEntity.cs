namespace Wms.Domain.Entities.Common;

public abstract class BaseImportLineEntity : BaseEntity
{
    public string StockCode { get; set; } = null!;
    public string? YapKod { get; set; }
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description { get; set; }
}

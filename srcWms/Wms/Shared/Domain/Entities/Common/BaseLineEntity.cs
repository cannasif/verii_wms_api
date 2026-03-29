namespace Wms.Domain.Entities.Common;

/// <summary>
/// `_old/reference/verii_wms_api.Domain/Entities/Common/BaseLineEntity.cs` satır tabanı davranışını taşır.
/// </summary>
public abstract class BaseLineEntity : BaseEntity
{
    public string StockCode { get; set; } = null!;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

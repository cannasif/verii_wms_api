namespace Wms.Domain.Entities.GoodsReceipt.Functions;

public sealed class FnGoodsOpenOrdersHeader
{
    public string Mode { get; set; } = string.Empty;
    public string SiparisNo { get; set; } = string.Empty;
    public long? OrderId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public short? BranchCode { get; set; }
    public short? TargetWh { get; set; }
    public string? ProjectCode { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? OrderedQty { get; set; }
    public decimal? DeliveredQty { get; set; }
    public decimal? RemainingHamax { get; set; }
    public decimal PlannedQtyAllocated { get; set; }
    public decimal? RemainingForImport { get; set; }
}

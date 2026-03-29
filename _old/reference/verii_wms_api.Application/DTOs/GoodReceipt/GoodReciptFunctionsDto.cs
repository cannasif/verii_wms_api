using System;

namespace WMS_WEBAPI.DTOs
{
    public class FN_GoodsOpenOrders_HeaderDto
    {
        public string Mode { get; set; } = string.Empty;
        public string SiparisNo { get; set; } = string.Empty;
        public int? OrderID { get; set; }
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

    public class FN_GoodsOpenOrders_LineDto
    {
        public string Mode { get; set; } = null!;
        public string SiparisNo { get; set; } = null!;
        public int OrderID { get; set; }
        public string? StockCode { get; set; }
        public string? StockName { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public short BranchCode { get; set; }
        public short? TargetWh { get; set; }
        public string? ProjectCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderedQty { get; set; }
        public decimal? DeliveredQty { get; set; }
        public decimal? RemainingHamax { get; set; }
        public decimal PlannedQtyAllocated { get; set; }
        public decimal? RemainingForImport { get; set; }
    }
}

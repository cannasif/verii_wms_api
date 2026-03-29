using System;

namespace WMS_WEBAPI.Models
{
    public class FN_SitOpenOrder_Header
    {
        public string Mode { get; set; } = null!;

        public string SiparisNo { get; set; } = null!;

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
}

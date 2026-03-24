using System;

namespace WMS_WEBAPI.Models
{
    public class FN_TransferOpenOrder_Line
    {
        public string Mode { get; set; } = null!;

        public string SiparisNo { get; set; } = null!;

        public int OrderID { get; set; }
        public string? StockCode { get; set; }                  // varchar(35), nullable
        public string? StockName { get; set; }                  // varchar(35), nullable
        public string? YapKod { get; set; }                     // varchar(35), nullable
        public string? YapAcik { get; set; }                    // varchar(35), nullable
        public string? CustomerCode { get; set; }

        public string? CustomerName { get; set; }

        public int BranchCode { get; set; }

        public int? TargetWh { get; set; }

        public string? ProjectCode { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? OrderedQty { get; set; }

        public decimal? DeliveredQty { get; set; }

        public decimal? RemainingHamax { get; set; }

        public decimal PlannedQtyAllocated { get; set; }

        public decimal? RemainingForImport { get; set; }
    }
}

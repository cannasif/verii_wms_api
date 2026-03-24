using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseRouteEntity : BaseEntity
    {
        public string ScannedBarcode { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string? Description { get; set; }

        // Seri bilgileri (isteğe bağlı, kalabilir veya kaldırılabilir)
        public string? SerialNo { get; set; }
        
        public string? SerialNo2 { get; set; }

        public string? SerialNo3 { get; set; }

        public string? SerialNo4 { get; set; }

        // Location details
        public int? SourceWarehouse { get; set; }
        public int? TargetWarehouse { get; set; }

        public string? SourceCellCode { get; set; }

        public string? TargetCellCode { get; set; }

    }
}

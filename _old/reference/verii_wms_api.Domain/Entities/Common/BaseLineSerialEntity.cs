using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseLineSerialEntity : BaseEntity
    {

        public decimal Quantity { get; set; }

        // Seri bilgileri (isteğe bağlı, kalabilir veya kaldırılabilir)
        public string? SerialNo { get; set; }
        
        public string? SerialNo2 { get; set; }

        public string? SerialNo3 { get; set; }

        public string? SerialNo4 { get; set; }

        public string? SourceCellCode { get; set; }

        public string? TargetCellCode { get; set; }

    }
}

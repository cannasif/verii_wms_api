using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PrHeader : BaseHeaderEntity
    {
        public string? CustomerCode { get; set; }

        public string? StockCode { get; set; }

        public string? YapKod { get; set; }

        // Üretim miktarı
        public decimal? Quantity { get; set; }

        public string? SourceWarehouse { get; set; }

        public string? TargetWarehouse { get; set; }


        public virtual ICollection<PrLine> Lines { get; set; } = new List<PrLine>();
        public virtual ICollection<PrHeaderSerial> HeaderSerials { get; set; } = new List<PrHeaderSerial>();
        public virtual ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
        public virtual ICollection<PrTerminalLine> TerminalLines { get; set; } = new List<PrTerminalLine>();
    }
}

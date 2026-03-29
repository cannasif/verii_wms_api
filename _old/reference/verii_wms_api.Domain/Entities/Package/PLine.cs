using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PLine : BaseEntity
    {

        public long PackingHeaderId { get; set; }
        public virtual PHeader PackingHeader { get; set; } = null!;

        public long PackageId { get; set; }
        public virtual PPackage Package { get; set; } = null!;

        public string? Barcode { get; set; }

        public string StockCode { get; set; } = null!;

        public string? YapKod { get; set; }

        public decimal Quantity { get; set; }

        public string? SerialNo { get; set; }

        public string? SerialNo2 { get; set; }

        public string? SerialNo3 { get; set; }

        public string? SerialNo4 { get; set; }
        
        public long? SourceRouteId { get; set; }
        
    }
}

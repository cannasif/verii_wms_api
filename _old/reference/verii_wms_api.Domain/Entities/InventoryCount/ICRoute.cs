using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class IcRoute : BaseEntity
    {

        public long ImportLineId { get; set; }
        public virtual IcImportLine ImportLine { get; set; } = null!;

        public string? Barcode { get; set; }

        public decimal OldQuantity { get; set; }

        public decimal NewQuantity { get; set; }

        public string? SerialNo { get; set; }

        public string? SerialNo2 { get; set; }

        public int? OldWarehouse { get; set; }

        public int? NewWarehouse { get; set; }

        public string? OldCellCode { get; set; }

        public string? NewCellCode { get; set; }

        public string? Description { get; set; }
    }
}

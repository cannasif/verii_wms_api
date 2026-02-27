using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_IC_ROUTE")]
    public class IcRoute : BaseEntity
    {

        [Required, ForeignKey(nameof(ImportLine))]
        public long ImportLineId { get; set; }
        public virtual IcImportLine ImportLine { get; set; } = null!;

        [Required, MaxLength(50)]
        public string? Barcode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal OldQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal NewQuantity { get; set; }

        [MaxLength(50)]
        public string? SerialNo { get; set; }

        [MaxLength(50)]
        public string? SerialNo2 { get; set; }

        public int? OldWarehouse { get; set; }

        public int? NewWarehouse { get; set; }

        [MaxLength(20)]
        public string? OldCellCode { get; set; }

        [MaxLength(20)]
        public string? NewCellCode { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}

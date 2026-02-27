using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WT_HEADER")]
    public class WtHeader : BaseHeaderEntity
    {
        [MaxLength(20)]
        public string? CustomerCode { get; set; }

        [MaxLength(20)]
        public string? SourceWarehouse { get; set; }

        [MaxLength(20)]
        public string? TargetWarehouse { get; set; }

        [Required]
        public bool ElectronicWaybill { get; set; } = false; // Elektronik yolcu reçetesi

        public long? ShipmentId { get; set; }

        // Navigation properties
        public virtual ICollection<WtLine> Lines { get; set; } = new List<WtLine>();
        public virtual ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
        public virtual ICollection<WtTerminalLine> TerminalLines { get; set; } = new List<WtTerminalLine>();
        
    }
}

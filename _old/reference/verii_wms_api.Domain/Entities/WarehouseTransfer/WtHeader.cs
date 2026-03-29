using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WtHeader : BaseHeaderEntity
    {
        public string? CustomerCode { get; set; }

        public string? SourceWarehouse { get; set; }

        public string? TargetWarehouse { get; set; }

        public bool ElectronicWaybill { get; set; } = false; // Elektronik yolcu reçetesi

        public long? ShipmentId { get; set; }

        // Navigation properties
        public virtual ICollection<WtLine> Lines { get; set; } = new List<WtLine>();
        public virtual ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
        public virtual ICollection<WtTerminalLine> TerminalLines { get; set; } = new List<WtTerminalLine>();
        
    }
}

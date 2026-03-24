using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class ShHeader : BaseHeaderEntity
    {
        public string? CustomerCode { get; set; }

        public string? SourceWarehouse { get; set; }

        public string? TargetWarehouse { get; set; }

        public long? ShipmentId { get; set; }

        public virtual ICollection<ShLine> Lines { get; set; } = new List<ShLine>();
        public virtual ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
        public virtual ICollection<ShTerminalLine> TerminalLines { get; set; } = new List<ShTerminalLine>();
    }
}

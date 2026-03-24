using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class ShLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual ShHeader Header { get; set; } = null!;

        public virtual ICollection<ShImportLine> ImportLines { get; set; } = new List<ShImportLine>();
        public virtual ICollection<ShLineSerial> SerialLines { get; set; } = new List<ShLineSerial>();
    }
}

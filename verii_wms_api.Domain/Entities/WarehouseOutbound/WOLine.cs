using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WoLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual WoHeader Header { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<WoImportLine> ImportLines { get; set; } = new List<WoImportLine>();
        public virtual ICollection<WoLineSerial> SerialLines { get; set; } = new List<WoLineSerial>();

    }
}

using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PrLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual PrHeader Header { get; set; } = null!;

        public virtual ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
        public virtual ICollection<PrLineSerial> SerialLines { get; set; } = new List<PrLineSerial>();
    }
}

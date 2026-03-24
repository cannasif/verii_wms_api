using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WiLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual WiHeader Header { get; set; } = null!;

        public virtual ICollection<WiImportLine> ImportLines { get; set; } = new List<WiImportLine>();
        public virtual ICollection<WiLineSerial> SerialLines { get; set; } = new List<WiLineSerial>();

    }
}

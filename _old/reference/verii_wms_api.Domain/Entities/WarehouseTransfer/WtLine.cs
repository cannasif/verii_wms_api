using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual WtHeader Header { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
        public virtual ICollection<WtLineSerial> SerialLines { get; set; } = new List<WtLineSerial>();

    }
}
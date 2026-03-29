using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class GrLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual GrHeader Header { get; set; } = null!;

        public virtual ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
        public virtual ICollection<GrLineSerial> SerialLines { get; set; } = new List<GrLineSerial>();

    }
}

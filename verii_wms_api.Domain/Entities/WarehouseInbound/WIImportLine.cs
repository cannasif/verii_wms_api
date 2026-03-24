using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WiImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual WiHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual WiLine? Line { get; set; }

    }
}

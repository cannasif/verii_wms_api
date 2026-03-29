using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PtImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual PtHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual PtLine? Line { get; set; }

    }
}

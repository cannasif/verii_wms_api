using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SitImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual SitHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual SitLine? Line { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SrtImportLine : BaseImportLineEntity
    {

        public long HeaderId { get; set; }
        public virtual SrtHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual SrtLine? Line { get; set; }


    }
}

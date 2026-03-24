using System;
using System.Collections.Generic;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models
{
    public class GrImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual GrHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual GrLine? Line { get; set; }
        
        //navigation properties
        public virtual ICollection<GrRoute> Routes { get; set; } = new List<GrRoute>();
    }
}

using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WoImportLine : BaseImportLineEntity
    {

        public long HeaderId { get; set; }
        public virtual WoHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual WoLine? Line { get; set; }

        // Navigation properties
        public virtual ICollection<WoRoute> Routes { get; set; } = new List<WoRoute>();
    }
}

using System;

namespace WMS_WEBAPI.Models
{
    public class PrImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual PrHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual PrLine? Line { get; set; }

        public virtual ICollection<PrRoute> Routes { get; set; } = new List<PrRoute>();
    }
}

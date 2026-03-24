using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class ShImportLine : BaseImportLineEntity
    {
        public long HeaderId { get; set; }
        public virtual ShHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        public virtual ShLine Line { get; set; } = null!;

        public virtual ICollection<ShRoute> Routes { get; set; } = new List<ShRoute>();
    }
}

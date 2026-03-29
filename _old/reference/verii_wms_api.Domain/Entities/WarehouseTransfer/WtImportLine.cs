using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WtImportLine : BaseImportLineEntity
    {
        // Header ilişkisi
        public long HeaderId { get; set; }
        public virtual WtHeader Header { get; set; } = null!;

        // Üst işlem (Line)
        public long? LineId { get; set; }
        public virtual WtLine Line { get; set; } = null!;

       // Navigation properties
        public virtual ICollection<WtRoute> Routes { get; set; } = new List<WtRoute>();

    }
}

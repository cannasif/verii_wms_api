using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WO_IMPORT_LINE")]
    public class WoImportLine : BaseImportLineEntity
    {

        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual WoHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual WoLine? Line { get; set; }

        // Navigation properties
        public virtual ICollection<WoRoute> Routes { get; set; } = new List<WoRoute>();
    }
}

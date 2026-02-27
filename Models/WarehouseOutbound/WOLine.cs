using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WO_LINE")]
    public class WoLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual WoHeader Header { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<WoImportLine> ImportLines { get; set; } = new List<WoImportLine>();
        public virtual ICollection<WoLineSerial> SerialLines { get; set; } = new List<WoLineSerial>();

    }
}

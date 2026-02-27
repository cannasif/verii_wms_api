using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WI_LINE")]
    public class WiLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual WiHeader Header { get; set; } = null!;

        public virtual ICollection<WiImportLine> ImportLines { get; set; } = new List<WiImportLine>();
        public virtual ICollection<WiLineSerial> SerialLines { get; set; } = new List<WiLineSerial>();

    }
}

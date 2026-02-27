using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WT_LINE")]
    public class WtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual WtHeader Header { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<WtImportLine> ImportLines { get; set; } = new List<WtImportLine>();
        public virtual ICollection<WtLineSerial> SerialLines { get; set; } = new List<WtLineSerial>();

    }
}
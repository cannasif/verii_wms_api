using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_GR_LINE")]
    public class GrLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual GrHeader Header { get; set; } = null!;

        public virtual ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
        public virtual ICollection<GrLineSerial> SerialLines { get; set; } = new List<GrLineSerial>();

    }
}

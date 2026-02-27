using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    // RII_PT_LINE tablosu: Üretim Transfer (Production Transfer) veya Üretim Satırı kayıtlarını tutar
    [Table("RII_PT_LINE")]
    public class PtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual PtHeader Header { get; set; } = null!;

        public virtual ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
        public virtual ICollection<PtLineSerial> SerialLines { get; set; } = new List<PtLineSerial>();

    }
}

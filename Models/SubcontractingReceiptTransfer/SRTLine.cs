using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    // RII_SRT_LINE tablosu: Subcontracting Transfer (Subcontracting Transfer) veya Subcontracting Satırı kayıtlarını tutar
    [Table("RII_SRT_LINE")]
    public class SrtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual SrtHeader Header { get; set; } = null!;

        public virtual ICollection<SrtImportLine> ImportLines { get; set; } = new List<SrtImportLine>();
        public virtual ICollection<SrtLineSerial> SerialLines { get; set; } = new List<SrtLineSerial>();
    }
}

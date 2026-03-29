using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_SRT_LINE tablosu: Subcontracting Transfer (Subcontracting Transfer) veya Subcontracting Satırı kayıtlarını tutar
    public class SrtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual SrtHeader Header { get; set; } = null!;

        public virtual ICollection<SrtImportLine> ImportLines { get; set; } = new List<SrtImportLine>();
        public virtual ICollection<SrtLineSerial> SerialLines { get; set; } = new List<SrtLineSerial>();
    }
}

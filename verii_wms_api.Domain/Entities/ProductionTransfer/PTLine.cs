using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_PT_LINE tablosu: Üretim Transfer (Production Transfer) veya Üretim Satırı kayıtlarını tutar
    public class PtLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual PtHeader Header { get; set; } = null!;

        public virtual ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
        public virtual ICollection<PtLineSerial> SerialLines { get; set; } = new List<PtLineSerial>();

    }
}

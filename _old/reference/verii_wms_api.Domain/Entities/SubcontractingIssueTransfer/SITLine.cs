using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_SIT_LINE tablosu: Üretim Transfer (Production Transfer) veya Üretim Satırı kayıtlarını tutar
    public class SitLine : BaseLineEntity
    {
        public long HeaderId { get; set; }
        public virtual SitHeader Header { get; set; } = null!;

        public virtual ICollection<SitImportLine> ImportLines { get; set; } = new List<SitImportLine>();
        public virtual ICollection<SitLineSerial> SerialLines { get; set; } = new List<SitLineSerial>();
        
    }
}

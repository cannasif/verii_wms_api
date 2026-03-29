using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_PT_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (PtLine) içerir.
    public class SitHeader : BaseHeaderEntity
    {

        // Müşteri kodu – genellikle fason üretim veya dış kaynak durumlarında kullanılır (ERP cari kodu)
        public string? CustomerCode { get; set; }

        // Kaynak depo kodu – ürünlerin çıkış yaptığı depo
        public string? SourceWarehouse { get; set; }

        // Hedef depo kodu – ürünlerin giriş yaptığı depo
        public string? TargetWarehouse { get; set; }


        // Navigation properties ↓
        public virtual ICollection<SitLine> Lines { get; set; } = new List<SitLine>();
        public virtual ICollection<SitImportLine> ImportLines { get; set; } = new List<SitImportLine>();
        public virtual ICollection<SitTerminalLine> TerminalLines { get; set; } = new List<SitTerminalLine>();
        
    }
}

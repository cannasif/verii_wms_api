using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_WI_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (WoLine) içerir.
    public class WiHeader : BaseHeaderEntity
    {
        // 1 = Sevk İrsaliyesi
        // 2 = Fire Çıkış
        // 3 = Düzeltme Çıkış
        // 4 = Transfer ile Çıkış
        public string InboundType { get; set; } = null!;

        // Hesap kodu – genellikle finansal işlemler için kullanılır (örnek: "1000")
        public string? AccountCode { get; set; }

        // Müşteri kodu – genellikle fason üretim veya dış kaynak durumlarında kullanılır (ERP cari kodu)
        public string? CustomerCode { get; set; }

        // Kaynak depo kodu – ürünlerin çıkış yaptığı depo
        public string? SourceWarehouse { get; set; }

        // Hedef depo kodu – ürünlerin giriş yaptığı depo
        public string? TargetWarehouse { get; set; }


        // Navigation properties ↓
        public virtual ICollection<WiLine> Lines { get; set; } = new List<WiLine>();
        public virtual ICollection<WiImportLine> ImportLines { get; set; } = new List<WiImportLine>();
        public virtual ICollection<WiTerminalLine> TerminalLines { get; set; } = new List<WiTerminalLine>();

    }
}

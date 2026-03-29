using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    // RII_IC_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (PtLine) içerir.
    public class IcHeader : BaseHeaderEntity
    {  
       // Depo kodu – ürünlerin sayım yaptığı depo
        public string? WarehouseCode { get; set; }



        // Ürün kodu – sayım yapılacak ürünün kodu (örn. "M12345")
        public string? ProductCode { get; set; }

        public string? CellCode { get; set; }

        // Sayım tipi (örnek: 1 = Tam Sayım, 2 = Kısmi Sayım, 3 = Döngü Sayım)
        public byte Type { get; set; }

        public virtual ICollection<IcTerminalLine> TerminalLines { get; set; } = new List<IcTerminalLine>();
        public virtual ICollection<IcImportLine> ImportLines { get; set; } = new List<IcImportLine>();

    }
}

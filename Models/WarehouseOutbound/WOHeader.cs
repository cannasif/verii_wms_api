using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    // RII_WO_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (WoLine) içerir.
    [Table("RII_WO_HEADER")]
    public class WoHeader : BaseHeaderEntity
    {

        // 1 = Sevk İrsaliyesi
        // 2 = Fire Çıkış
        // 3 = Düzeltme Çıkış
        // 4 = Transfer ile Çıkış
        [Required, MaxLength(10)]
        public string OutboundType { get; set; } = null!;

        // Hesap kodu – genellikle finansal işlemler için kullanılır (örnek: "1000")
        [MaxLength(20)]
        public string? AccountCode { get; set; }

        // Müşteri kodu – genellikle fason üretim veya dış kaynak durumlarında kullanılır (ERP cari kodu)
        [MaxLength(20)]
        public string? CustomerCode { get; set; }

        // Kaynak depo kodu – ürünlerin çıkış yaptığı depo
        [MaxLength(20)]
        public string? SourceWarehouse { get; set; }

        // Hedef depo kodu – ürünlerin giriş yaptığı depo
        [MaxLength(20)]
        public string? TargetWarehouse { get; set; }

        // Kayıt tipi (örnek: 0 = üretim transfer, 1 = fason, 2 = üretim çıkışı)
        [Required]
        public byte Type { get; set; }

         // Navigation properties
        public virtual ICollection<WoLine> Lines { get; set; } = new List<WoLine>();
        public virtual ICollection<WoImportLine> ImportLines { get; set; } = new List<WoImportLine>();
        public virtual ICollection<WoTerminalLine> TerminalLines { get; set; } = new List<WoTerminalLine>();
    }
}

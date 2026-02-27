using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    // RII_IC_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (PtLine) içerir.
    [Table("RII_IC_HEADER")]
    public class IcHeader : BaseHeaderEntity
    {  
       // Depo kodu – ürünlerin sayım yaptığı depo
        [MaxLength(20)]
        public string? WarehouseCode { get; set; }



        // Ürün kodu – sayım yapılacak ürünün kodu (örn. "M12345")
        [MaxLength(50)]
        public string? ProductCode { get; set; }

        [MaxLength(35)]
        public string? CellCode { get; set; }

        // Sayım tipi (örnek: 1 = Tam Sayım, 2 = Kısmi Sayım, 3 = Döngü Sayım)
        [Required]
        public byte Type { get; set; }

        public virtual ICollection<IcTerminalLine> TerminalLines { get; set; } = new List<IcTerminalLine>();
        public virtual ICollection<IcImportLine> ImportLines { get; set; } = new List<IcImportLine>();

    }
}

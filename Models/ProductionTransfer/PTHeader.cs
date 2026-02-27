using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    // RII_PT_HEADER tablosu:
    // Üretim transfer fişinin veya üretim hareketinin üst bilgilerini (header) tutar.
    // Her header birden fazla satır (PtLine) içerir.
    [Table("RII_PT_HEADER")]
    public class PtHeader : BaseHeaderEntity
    {

        // Müşteri kodu – genellikle fason üretim veya dış kaynak durumlarında kullanılır (ERP cari kodu)
        [MaxLength(20)]
        public string? CustomerCode { get; set; }

        // Kaynak depo kodu – ürünlerin çıkış yaptığı depo
        [MaxLength(20)]
        public string? SourceWarehouse { get; set; }

        // Hedef depo kodu – ürünlerin giriş yaptığı depo
        [MaxLength(20)]
        public string? TargetWarehouse { get; set; }

        // Navigation properties ↓
        public virtual ICollection<PtLine> Lines { get; set; } = new List<PtLine>();
        public virtual ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
        public virtual ICollection<PtTerminalLine> TerminalLines { get; set; } = new List<PtTerminalLine>();
        
    }
}

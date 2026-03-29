using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SrtHeader :  BaseHeaderEntity
    {

        // Müşteri kodu – genellikle fason üretim veya dış kaynak durumlarında kullanılır (ERP cari kodu)
        public string? CustomerCode { get; set; }

        // Kaynak depo kodu – ürünlerin çıkış yaptığı depo
        public string? SourceWarehouse { get; set; }

        // Hedef depo kodu – ürünlerin giriş yaptığı depo
        public string? TargetWarehouse { get; set; }


        // Navigation properties ↓
        public virtual ICollection<SrtLine> Lines { get; set; } = new List<SrtLine>();
        public virtual ICollection<SrtImportLine> ImportLines { get; set; } = new List<SrtImportLine>();
        public virtual ICollection<SrtTerminalLine> TerminalLines { get; set; } = new List<SrtTerminalLine>();

    }
}

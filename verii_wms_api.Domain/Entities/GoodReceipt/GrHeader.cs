using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class GrHeader : BaseHeaderEntity
    {

        public string CustomerCode { get; set; } = null!; // Müşteri kodu

        public bool ElectronicWaybill { get; set; } = false; // Elektronik yolcu reçetesi
        
        public bool ReturnCode { get; set; } = false; // İade işlemi mi?
        public bool OCRSource { get; set; } = false; // OCR’dan mı geldi?

        // 🔸 Açıklama alanları
        public string? Description3 { get; set; }
        public string? Description4 { get; set; }
        public string? Description5 { get; set; }

        // 🔗 İlişkiler (Navigation Properties)
        public virtual ICollection<GrLine> Lines { get; set; } = new List<GrLine>();
        public virtual ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
        
    }
}

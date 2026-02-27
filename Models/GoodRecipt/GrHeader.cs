using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_GR_HEADER")]
    public class GrHeader : BaseHeaderEntity
    {

        [Required, StringLength(30)]
        public string CustomerCode { get; set; } = null!; // Müşteri kodu

        [Required]
        public bool ElectronicWaybill { get; set; } = false; // Elektronik yolcu reçetesi
        
        public bool ReturnCode { get; set; } = false; // İade işlemi mi?
        public bool OCRSource { get; set; } = false; // OCR’dan mı geldi?

        // 🔸 Açıklama alanları
        [StringLength(50)]
        public string? Description3 { get; set; }
        [StringLength(100)]
        public string? Description4 { get; set; }
        [StringLength(100)]
        public string? Description5 { get; set; }

        // 🔗 İlişkiler (Navigation Properties)
        public virtual ICollection<GrLine> Lines { get; set; } = new List<GrLine>();
        public virtual ICollection<GrImportLine> ImportLines { get; set; } = new List<GrImportLine>();
        
    }
}

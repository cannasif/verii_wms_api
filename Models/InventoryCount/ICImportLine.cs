using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_IC_IMPORT_LINE")]
    public class IcImportLine : BaseEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual IcHeader Header { get; set; } = null!;


        // ÜRÜN / MALZEME BİLGİLERİ
        // Stok kodu – ERP veya WMS’deki ürün referansı
        [Required, MaxLength(35)]
        public string StockCode { get; set; } = null!;

        [MaxLength(35)]
        public string? YapKod { get; set; } = null;

        // AÇIKLAMA ALANLARI
        // İşlemi açıklayan ek bilgiler (örneğin fason açıklaması, terminal notu)
        [MaxLength(30)]
        public string? Description1 { get; set; }

        [MaxLength(50)]
        public string? Description2 { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<IcRoute> Routes { get; set; } = new List<IcRoute>();

    }
}

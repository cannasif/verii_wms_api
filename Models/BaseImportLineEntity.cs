using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseImportLineEntity : BaseEntity
    {
        // Ürün bilgileri
        [Required, MaxLength(50)]
        public string StockCode { get; set; } = null!;

        [MaxLength(50)]
        public string? YapKod { get; set; } = null;

        // AÇIKLAMA ALANLARI
        // İşlemi açıklayan ek bilgiler
        [MaxLength(100)]
        public string? Description1 { get; set; }

        [MaxLength(100)]
        public string? Description2 { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

    }
}

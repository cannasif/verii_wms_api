using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseImportLineEntity : BaseEntity
    {
        // Ürün bilgileri
        public string StockCode { get; set; } = null!;

        public string? YapKod { get; set; } = null;

        // AÇIKLAMA ALANLARI
        // İşlemi açıklayan ek bilgiler
        public string? Description1 { get; set; }

        public string? Description2 { get; set; }

        public string? Description { get; set; }

    }
}

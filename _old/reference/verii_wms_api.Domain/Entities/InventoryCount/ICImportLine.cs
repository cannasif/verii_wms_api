using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class IcImportLine : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual IcHeader Header { get; set; } = null!;


        // ÜRÜN / MALZEME BİLGİLERİ
        // Stok kodu – ERP veya WMS’deki ürün referansı
        public string StockCode { get; set; } = null!;

        public string? YapKod { get; set; } = null;

        // AÇIKLAMA ALANLARI
        // İşlemi açıklayan ek bilgiler (örneğin fason açıklaması, terminal notu)
        public string? Description1 { get; set; }

        public string? Description2 { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<IcRoute> Routes { get; set; } = new List<IcRoute>();

    }
}

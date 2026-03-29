using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PPackage : BaseEntity
    {
        // PackingHeader Foreign Key
        public long PackingHeaderId { get; set; }
        public virtual PHeader PackingHeader { get; set; } = null!;

        // Paket numarası (Koli-1, Palet-2)
        public string PackageNo { get; set; } = null!;

        // Paket tipi: Box / Pallet / Bag / Custom
        public string PackageType { get; set; } = PPackageType.Box;

        // Koli barkodu (SSCC uyumlu olabilir)
        public string? Barcode { get; set; }

        // Boyut bilgileri
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }

        // Ağırlık bilgileri
        public decimal? NetWeight { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? GrossWeight { get; set; }

        // Farklı ürün içeriyor mu
        public bool IsMixed { get; set; } = false;

        // Durum: Open / Closed / Loaded
        public string Status { get; set; } = PPackageStatus.Open;

        // Navigasyon özellikleri
        public virtual ICollection<PLine> Lines { get; set; } = new List<PLine>();
    }
}

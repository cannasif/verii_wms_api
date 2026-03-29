using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PHeader : BaseEntity
    {

        public string? WarehouseCode { get; set; }

        // Paket numarası (PKG-2025-000123 formatında)
        public string PackingNo { get; set; } = null!;

        // Paketleme tarihi
        public DateTime? PackingDate { get; set; }

        // Kaynak tipi: SalesOrder / Transfer / Production / Return
        public string? SourceType { get; set; }

        // Kaynak Header ID (İlgili belgenin header Id'si)
        public long? SourceHeaderId { get; set; }

        // Müşteri bilgileri
        public string? CustomerCode { get; set; }
        public string? CustomerAddress { get; set; }

        // Durum: Draft / Packing / Packed / Shipped / Cancelled
        public string Status { get; set; } = PHeaderStatus.Draft;

        // Toplam bilgileri
        public decimal? TotalPackageCount { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalNetWeight { get; set; }
        public decimal? TotalGrossWeight { get; set; }
        public decimal? TotalVolume { get; set; }

        // Kargo bilgileri
        public long? CarrierId { get; set; }
        public string? CarrierServiceType { get; set; }
        public string? TrackingNo { get; set; }

        // Eşleşme durumu
        public bool IsMatched { get; set; } = false;

        // Navigasyon özellikleri
        public virtual ICollection<PPackage> Packages { get; set; } = new List<PPackage>();
    }
}


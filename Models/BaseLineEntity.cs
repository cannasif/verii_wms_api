using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseLineEntity : BaseEntity
    {

        // Stok kodu – ERP veya WMS tarafındaki malzeme/ürün kodu
        [Required, MaxLength(35)]
        public string StockCode { get; set; } = null!;

        public string? YapKod { get; set; } = null;

        // Satırdaki miktar – üretime gönderilen ya da üretilen miktar
        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; }

        // Miktarın birimi (örneğin KG, ADET, MTR)
        [MaxLength(10)]
        public string? Unit { get; set; }

        // ERP alanları ↓
        // ERP'deki üretim veya transfer emri numarası (örneğin Netsis Üretim Emri No)
        [MaxLength(50)]
        public string? ErpOrderNo { get; set; }

        // ERP emrinin satır numarası (örneğin "001", "002")
        [MaxLength(30)]
        public string? ErpOrderId { get; set; }


        // Satır açıklaması – stokun, operasyonun veya üretim işinin açıklaması olabilir
        [MaxLength(100)]
        public string? Description { get; set; }

    }
}

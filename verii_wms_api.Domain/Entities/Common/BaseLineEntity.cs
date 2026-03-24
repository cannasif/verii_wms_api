using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseLineEntity : BaseEntity
    {

        // Stok kodu – ERP veya WMS tarafındaki malzeme/ürün kodu
        public string StockCode { get; set; } = null!;

        public string? YapKod { get; set; } = null;

        // Satırdaki miktar – üretime gönderilen ya da üretilen miktar
        public decimal Quantity { get; set; }

        // Miktarın birimi (örneğin KG, ADET, MTR)
        public string? Unit { get; set; }

        // ERP alanları ↓
        // ERP'deki üretim veya transfer emri numarası (örneğin Netsis Üretim Emri No)
        public string? ErpOrderNo { get; set; }

        // ERP emrinin satır numarası (örneğin "001", "002")
        public string? ErpOrderId { get; set; }


        // Satır açıklaması – stokun, operasyonun veya üretim işinin açıklaması olabilir
        public string? Description { get; set; }

    }
}

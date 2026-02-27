using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
        public string? ErpLineReference { get; set; }
    }

    public class CreateSrtLineDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        [StringLength(35)]
        public string StockCode { get; set; } = string.Empty;

        public int? OrderId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [StringLength(10)]
        public string? Unit { get; set; }

        [StringLength(50)]
        public string? ErpOrderNo { get; set; }

        [StringLength(10)]
        public string? ErpOrderId { get; set; }

        [StringLength(10)]
        public string? ErpLineReference { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
    }

    public class UpdateSrtLineDto
    {
        public long? HeaderId { get; set; }

        [StringLength(35)]
        public string? StockCode { get; set; }

        public int? OrderId { get; set; }

        public decimal? Quantity { get; set; }

        [StringLength(10)]
        public string? Unit { get; set; }

        [StringLength(50)]
        public string? ErpOrderNo { get; set; }

        [StringLength(10)]
        public string? ErpOrderId { get; set; }

        [StringLength(10)]
        public string? ErpLineReference { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
    }
}

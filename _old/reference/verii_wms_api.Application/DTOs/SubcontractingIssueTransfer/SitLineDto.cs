using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
        public string? ErpLineReference { get; set; }
    }

    public class CreateSitLineDto : BaseLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        public int? OrderId { get; set; }

        [StringLength(10)]
        public string? ErpLineReference { get; set; }
    }

    public class UpdateSitLineDto : BaseLineUpdateDto
    {
        public long? HeaderId { get; set; }

        public int? OrderId { get; set; }

        [StringLength(10)]
        public string? ErpLineReference { get; set; }
    }
}
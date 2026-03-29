using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class IcRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
        public string YapKod { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string? Description { get; set; }
    }

    public class CreateIcRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
        
        [StringLength(30)]
        public string YapKod { get; set; } = string.Empty;
        
        public int Priority { get; set; }
        
        [StringLength(100)]
        public string? Description { get; set; }
    }

    public class UpdateIcRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
        
        [StringLength(30)]
        public string? YapKod { get; set; }
        
        public int? Priority { get; set; }
        
        [StringLength(100)]
        public string? Description { get; set; }
    }
}

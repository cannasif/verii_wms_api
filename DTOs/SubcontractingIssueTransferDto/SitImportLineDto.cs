using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
    }

    public class CreateSitImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }
    }

    public class UpdateSitImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }
    }
}
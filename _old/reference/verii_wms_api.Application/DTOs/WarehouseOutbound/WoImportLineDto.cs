using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WoImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
    }

    public class CreateWoImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }
    }

    public class UpdateWoImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }
    }
}
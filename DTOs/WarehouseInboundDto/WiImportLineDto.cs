using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WiImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
    }

    public class CreateWiImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }
    }

    public class UpdateWiImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }
    }
}
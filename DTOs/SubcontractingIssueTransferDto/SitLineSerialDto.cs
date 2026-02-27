using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SitLineSerialDto : BaseLineSerialEntityDto
    {
        public long LineId { get; set; }
    }

    public class CreateSitLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long LineId { get; set; }
    }

    public class UpdateSitLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtLineSerialDto : BaseLineSerialEntityDto
    {
        public long LineId { get; set; }
    }

    public class CreateSrtLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long LineId { get; set; }
    }

    public class UpdateSrtLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WiLineSerialDto : BaseLineSerialEntityDto
    {
        public long LineId { get; set; }
    }

    public class CreateWiLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long LineId { get; set; }
    }

    public class UpdateWiLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? LineId { get; set; }
    }
}
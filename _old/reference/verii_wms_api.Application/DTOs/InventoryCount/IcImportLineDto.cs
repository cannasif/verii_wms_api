using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class IcImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
    }

    public class CreateIcImportLineDto : BaseImportLineCreateDto
    {
        [Required]
        public long HeaderId { get; set; }
    }

    public class UpdateIcImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
    }
}
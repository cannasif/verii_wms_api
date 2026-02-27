using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrImportDocumentDto : BaseEntityDto
    {
        [Required(ErrorMessage = "HeaderId_Required")]
        public long HeaderId { get; set; }
        
        [Required(ErrorMessage = "Base64_Required")]
        public byte[] Base64 { get; set; } = null!;
    }

    public class CreateGrImportDocumentDto
    {
        [Required(ErrorMessage = "HeaderId_Required")]
        public long HeaderId { get; set; }
        
        [Required(ErrorMessage = "Base64_Required")]
        public byte[] Base64 { get; set; } = null!;
    }

    public class UpdateGrImportDocumentDto
    {
        [Required(ErrorMessage = "HeaderId_Required")]
        public long HeaderId { get; set; }
        
        [Required(ErrorMessage = "Base64_Required")]
        public byte[] Base64 { get; set; } = null!;
    }
}

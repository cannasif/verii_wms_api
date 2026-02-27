using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    /// <summary>
    /// GR Line DTO - Veri transfer objesi
    /// </summary>
    public class GrLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }

    }

    /// <summary>
    /// GR Line oluşturma DTO'su
    /// </summary>
    public class CreateGrLineDto : BaseLineCreateDto
    {
        [Required(ErrorMessage = "HeaderId alanı zorunludur")]
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
    }

    /// <summary>
    /// GR Line güncelleme DTO'su
    /// </summary>
    public class UpdateGrLineDto : BaseLineUpdateDto
    {
        [Required(ErrorMessage = "HeaderId alanı zorunludur")]
        public long HeaderId { get; set; }
        public int? OrderId { get; set; }
    }
}

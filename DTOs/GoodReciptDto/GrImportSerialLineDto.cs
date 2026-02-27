using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    /// <summary>
    /// GR Import Serial Line DTO - Veri transfer objesi
    /// </summary>
    public class GrImportSerialLineDto : BaseLineSerialEntityDto
    {
        public long ImportLineId { get; set; }
    }

    /// <summary>
    /// GR Import Serial Line oluşturma DTO'su
    /// </summary>
    public class CreateGrImportSerialLineDto : BaseLineSerialCreateDto
    {
        public long ImportLineId { get; set; }
    }

    /// <summary>
    /// GR Import Serial Line güncelleme DTO'su
    /// </summary>
    public class UpdateGrImportSerialLineDto : BaseLineSerialUpdateDto
    {
        public long ImportLineId { get; set; }
    }
}

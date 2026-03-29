using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtLineDto : BaseLineEntityDto
    {
        public long HeaderId { get; set; }
    }

    public class CreatePtLineDto : BaseLineCreateDto
    {
        public long HeaderId { get; set; }
    }

    public class UpdatePtLineDto : BaseLineUpdateDto
    {
        public long? HeaderId { get; set; }
    }

    
}
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtImportLineDto : BaseImportLineEntityDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
    }

    public class CreatePtImportLineDto : BaseImportLineCreateDto
    {
        public long HeaderId { get; set; }
        public long LineId { get; set; }
    }

    public class UpdatePtImportLineDto : BaseImportLineUpdateDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
    }

}
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class IcHeaderDto : BaseHeaderEntityDto
    {
        public string? DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string? CellCode { get; set; }
        public string? WarehouseCode { get; set; }
        public string? ProductCode { get; set; }
        public byte Type { get; set; }
    }

    public class CreateIcHeaderDto : BaseHeaderCreateDto
    {
        public string? DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        
        [StringLength(35)]
        public string? CellCode { get; set; }
        
        [StringLength(20)]
        public string? WarehouseCode { get; set; }
        
        [StringLength(50)]
        public string? ProductCode { get; set; }
        
        [Required]
        public byte Type { get; set; }
    }

    public class UpdateIcHeaderDto : BaseHeaderUpdateDto
    {
        public string? DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        
        [StringLength(35)]
        public string? CellCode { get; set; }
        
        [StringLength(20)]
        public string? WarehouseCode { get; set; }
        
        [StringLength(50)]
        public string? ProductCode { get; set; }
        
        public byte? Type { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class MobilePageGroupDto
    {
        public long Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public long? MenuHeaderId { get; set; }
        public long? MenuLineId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public long? DeletedBy { get; set; }
        
        // Full user information properties
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }

    }

    public class CreateMobilePageGroupDto
    {
        [Required(ErrorMessage = "GroupName is required")]
        [StringLength(100, ErrorMessage = "GroupName cannot exceed 100 characters")]
        public string GroupName { get; set; } = string.Empty;

        [Required(ErrorMessage = "GroupCode is required")]
        [StringLength(100, ErrorMessage = "GroupCode cannot exceed 100 characters")]
        public string GroupCode { get; set; } = string.Empty;

        public long? MenuHeaderId { get; set; }
        public long? MenuLineId { get; set; }
        public long? CreatedBy { get; set; }
    }

    public class UpdateMobilePageGroupDto
    {
        [StringLength(100, ErrorMessage = "GroupName cannot exceed 100 characters")]
        public string? GroupName { get; set; }

        [StringLength(100, ErrorMessage = "GroupCode cannot exceed 100 characters")]
        public string? GroupCode { get; set; }

        public long? MenuHeaderId { get; set; }
        public long? MenuLineId { get; set; }
        public long? UpdatedBy { get; set; }
    }
}

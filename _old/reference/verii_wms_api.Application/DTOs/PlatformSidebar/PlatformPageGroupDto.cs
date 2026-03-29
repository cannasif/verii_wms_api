using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PlatformPageGroupDto
    {
        public long Id { get; set; }
        
        [Required, MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

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

    public class CreatePlatformPageGroupDto
    {
        [Required, MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

    }

    public class UpdatePlatformPageGroupDto
    {
        [MaxLength(100)]
        public string? GroupName { get; set; }
        
        [MaxLength(100)]
        public string? GroupCode { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class MobileUserGroupMatchDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
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

    public class CreateMobileUserGroupMatchDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "GroupCode is required")]
        public string GroupCode { get; set; } = string.Empty;

        public long? CreatedBy { get; set; }
    }

    public class UpdateMobileUserGroupMatchDto
    {
        public long? UserId { get; set; }
        public string? GroupCode { get; set; }
        public long? UpdatedBy { get; set; }
    }
}

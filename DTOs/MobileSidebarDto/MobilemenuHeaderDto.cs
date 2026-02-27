using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class MobilemenuHeaderDto
    {
        public long Id { get; set; }
        public string MenuId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public bool IsOpen { get; set; }
        public DateTime CreatedDate { get; set; }
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

    public class CreateMobilemenuHeaderDto
    {
        [Required(ErrorMessage = "MenuId is required")]
        [StringLength(100, ErrorMessage = "MenuId cannot exceed 100 characters")]
        public string MenuId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
        public string? Icon { get; set; }

        public bool IsOpen { get; set; } = false;
        public string? CreatedBy { get; set; }
    }

    public class UpdateMobilemenuHeaderDto
    {
        [StringLength(100, ErrorMessage = "MenuId cannot exceed 100 characters")]
        public string? MenuId { get; set; }

        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string? Title { get; set; }

        [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
        public string? Icon { get; set; }

        public bool? IsOpen { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

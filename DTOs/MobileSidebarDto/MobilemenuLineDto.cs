using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class MobilemenuLineDto
    {
        public long Id { get; set; }
        public long HeaderId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string MenuCode { get; set; } = string.Empty;
        public string? MenuIcon { get; set; }
        public string? MenuUrl { get; set; }
        public int? OrderNo { get; set; }
        public bool IsActive { get; set; }
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

    public class CreateMobilemenuLineDto
    {
        [Required(ErrorMessage = "ItemId is required")]
        [StringLength(100, ErrorMessage = "ItemId cannot exceed 100 characters")]
        public string ItemId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
        public string? Icon { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "HeaderId is required")]
        public int HeaderId { get; set; }

        public string? CreatedBy { get; set; }
    }

    public class UpdateMobilemenuLineDto
    {
        [StringLength(100, ErrorMessage = "ItemId cannot exceed 100 characters")]
        public string? ItemId { get; set; }

        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string? Title { get; set; }

        [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
        public string? Icon { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public int? HeaderId { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

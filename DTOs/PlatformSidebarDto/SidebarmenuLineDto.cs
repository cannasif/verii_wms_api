using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SidebarmenuLineDto
    {
        public long Id { get; set; }
        
        [Required]
        public long HeaderId { get; set; }

        [Required, StringLength(100)]
        public string Page { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }
        
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

    public class CreateSidebarmenuLineDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required, StringLength(100)]
        public string Page { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }
    }

    public class UpdateSidebarmenuLineDto
    {
        public long? HeaderId { get; set; }

        [StringLength(100)]
        public string? Page { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }
    }
}

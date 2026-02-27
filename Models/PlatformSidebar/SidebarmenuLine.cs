using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PLATFORM_SIDEBARMENU_LINE")]
    public class SidebarmenuLine : BaseEntity
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

        // Navigation property
        [ForeignKey("HeaderId")]
        public virtual SidebarmenuHeader Header { get; set; } = null!;
    }
}

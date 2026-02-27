using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SidebarmenuHeaderDto
    {
        public long Id { get; set; }
        
        [Required, StringLength(50)]
        public string MenuKey { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Icon { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? DarkColor { get; set; }

        [StringLength(50)]
        public string? ShadowColor { get; set; }

        [StringLength(50)]
        public string? DarkShadowColor { get; set; }

        [StringLength(20)]
        public string? TextColor { get; set; }

        [StringLength(20)]
        public string? DarkTextColor { get; set; }

        public int RoleLevel { get; set; } = 0;
        
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

    public class CreateSidebarmenuHeaderDto
    {
        [Required, StringLength(50)]
        public string MenuKey { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Icon { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? DarkColor { get; set; }

        [StringLength(50)]
        public string? ShadowColor { get; set; }

        [StringLength(50)]
        public string? DarkShadowColor { get; set; }

        [StringLength(20)]
        public string? TextColor { get; set; }

        [StringLength(20)]
        public string? DarkTextColor { get; set; }

        public int RoleLevel { get; set; } = 0;
    }

    public class UpdateSidebarmenuHeaderDto
    {
        [StringLength(50)]
        public string? MenuKey { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(100)]
        public string? Icon { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? DarkColor { get; set; }

        [StringLength(50)]
        public string? ShadowColor { get; set; }

        [StringLength(50)]
        public string? DarkShadowColor { get; set; }

        [StringLength(20)]
        public string? TextColor { get; set; }

        [StringLength(20)]
        public string? DarkTextColor { get; set; }

        public int? RoleLevel { get; set; }
    }

    public class SidebarmenuHeaderWithLinesDto
    {
        public long Id { get; set; }
        public string MenuKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? DarkColor { get; set; }
        public string? ShadowColor { get; set; }
        public string? DarkShadowColor { get; set; }
        public string? TextColor { get; set; }
        public string? DarkTextColor { get; set; }
        public int RoleLevel { get; set; }
        public ICollection<SidebarmenuLineDto> Lines { get; set; } = new List<SidebarmenuLineDto>();
    }
}

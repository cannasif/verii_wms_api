using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PLATFORM_SIDEBARMENU_HEADER")]
    public class SidebarmenuHeader : BaseEntity
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

        // Navigation property
        public virtual ICollection<SidebarmenuLine> Lines { get; set; } = new List<SidebarmenuLine>();
        

    }
}

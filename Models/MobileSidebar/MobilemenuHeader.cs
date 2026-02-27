using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_MOBILMENU_HEADER")]
    public class MobilemenuHeader : BaseEntity
    {
        [Required, MaxLength(100)]
        public string MenuId { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Icon { get; set; }

        public bool IsOpen { get; set; } = false;

        // Navigation property
        public virtual ICollection<MobilemenuLine> Lines { get; set; } = new List<MobilemenuLine>();
    }
}

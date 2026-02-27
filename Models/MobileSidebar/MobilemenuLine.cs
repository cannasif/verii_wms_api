using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_MOBILMENU_LINE")]
    public class MobilemenuLine : BaseEntity
    {
        [Required, MaxLength(100)]
        public string ItemId { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Icon { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public long HeaderId { get; set; }

        [ForeignKey("HeaderId")]
        public virtual MobilemenuHeader Header { get; set; } = null!;
    }
}

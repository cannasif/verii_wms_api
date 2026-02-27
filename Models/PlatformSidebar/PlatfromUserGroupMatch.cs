using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PLATFORM_USER_GROUP_MATCH")]
    public class PlatformUserGroupMatch : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
       
        [Required]
        [MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

        // Navigation properties (ilişkiler DbContext'te tanımlanacak)
        public virtual User? Users { get; set; }
        public virtual PlatformPageGroup? Groups { get; set; }
    }
}

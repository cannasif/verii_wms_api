using System;

namespace WMS_WEBAPI.Models
{
    public class PlatformUserGroupMatch : BaseEntity
    {
        public long UserId { get; set; }
       
        public string GroupCode { get; set; } = string.Empty;

        // Navigation properties (ilişkiler DbContext'te tanımlanacak)
        public virtual User? Users { get; set; }
        public virtual PlatformPageGroup? Groups { get; set; }
    }
}

using System;

namespace WMS_WEBAPI.Models
{
    public class MobileUserGroupMatch : BaseEntity
    {
        public long UserId { get; set; }
            
        public string GroupCode { get; set; } = string.Empty;

        // Navigation properties (ilişkiler DbContext'te tanımlanacak)
        public virtual User? Users { get; set; }

        // Tek entity -> koleksiyon (list) yap
        public virtual ICollection<MobilePageGroup> MobilePageGroups { get; set; } = new List<MobilePageGroup>();

    }
}
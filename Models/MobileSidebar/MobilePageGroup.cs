using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_MOBIL_PAGE_GROUP")]
    public class MobilePageGroup : BaseEntity
    {

        [Required, MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

        // Foreign Keys
        public long? MenuHeaderId { get; set; }
        [Required,ForeignKey(nameof(MenuHeaderId))]
        public virtual MobilemenuHeader? MenuHeaders { get; set; }

        public long? MenuLineId { get; set; }
        [Required, ForeignKey(nameof(MenuLineId))]
        public virtual MobilemenuLine? MenuLines { get; set; }

        // Navigation property for many-to-many relationship
        public virtual ICollection<MobileUserGroupMatch> UserGroupMatches { get; set; } = new List<MobileUserGroupMatch>();
    }
}

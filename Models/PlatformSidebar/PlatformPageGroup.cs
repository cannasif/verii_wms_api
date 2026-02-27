using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PLATFORM_PAGE_GROUP")]
    public class PlatformPageGroup : BaseEntity
    {

        [Required, MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        
        [Required, MaxLength(100)]
        public string GroupCode { get; set; } = string.Empty;

        // Foreign Keys
        public long? MenuHeaderId { get; set; }
        [ForeignKey(nameof(MenuHeaderId))]
        public virtual SidebarmenuHeader? MenuHeaders { get; set; }

        public long? MenuLineId { get; set; }
        [ForeignKey(nameof(MenuLineId))]
        public virtual SidebarmenuLine? MenuLines { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SIT_ROUTE")]
    public class SitRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        [ForeignKey(nameof(ImportLineId))]
        public virtual SitImportLine ImportLine { get; set; } = null!;
        
    }
}

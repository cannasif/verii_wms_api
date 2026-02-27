using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SRT_ROUTE")]
    public class SrtRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        [ForeignKey(nameof(ImportLineId))]
        public virtual SrtImportLine ImportLine { get; set; } = null!;
    }
}

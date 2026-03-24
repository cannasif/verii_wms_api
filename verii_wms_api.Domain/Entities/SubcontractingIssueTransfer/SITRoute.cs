using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SitRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual SitImportLine ImportLine { get; set; } = null!;
        
    }
}

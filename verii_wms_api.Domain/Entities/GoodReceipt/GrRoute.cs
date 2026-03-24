using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class GrRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual GrImportLine ImportLine { get; set; } = null!;

    }
}

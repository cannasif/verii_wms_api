using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SrtRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual SrtImportLine ImportLine { get; set; } = null!;
    }
}

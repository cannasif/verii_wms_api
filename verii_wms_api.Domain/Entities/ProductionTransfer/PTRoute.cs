using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PtRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual PtImportLine ImportLine { get; set; } = null!;

    }
}

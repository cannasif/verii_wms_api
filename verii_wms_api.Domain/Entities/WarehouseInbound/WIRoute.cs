using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WiRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual WiImportLine ImportLine { get; set; } = null!;

    }
}

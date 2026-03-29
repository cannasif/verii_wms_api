using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WoRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual WoImportLine ImportLine { get; set; } = null!;

    }
}
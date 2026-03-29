using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WtRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual WtImportLine ImportLine { get; set; } = null!;

    }
}
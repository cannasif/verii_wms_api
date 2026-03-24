using System;

namespace WMS_WEBAPI.Models
{
    public class PrRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual PrImportLine ImportLine { get; set; } = null!;
    }
}

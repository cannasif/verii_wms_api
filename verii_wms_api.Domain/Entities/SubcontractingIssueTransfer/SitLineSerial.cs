using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SitLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual SitLine Line { get; set; } = null!;

    }
}
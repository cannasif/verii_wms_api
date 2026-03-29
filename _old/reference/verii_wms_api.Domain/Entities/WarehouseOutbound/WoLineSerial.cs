using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WoLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual WoLine Line { get; set; } = null!;

    }
}
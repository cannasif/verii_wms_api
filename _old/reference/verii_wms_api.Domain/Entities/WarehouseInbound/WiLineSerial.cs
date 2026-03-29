using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WiLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual WiLine Line { get; set; } = null!;

    }
}
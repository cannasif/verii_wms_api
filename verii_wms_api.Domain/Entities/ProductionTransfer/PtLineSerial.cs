using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class PtLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual PtLine Line { get; set; } = null!;

    }
}
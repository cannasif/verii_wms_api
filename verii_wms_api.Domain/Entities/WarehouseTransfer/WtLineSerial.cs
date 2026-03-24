using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class WtLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual WtLine Line { get; set; } = null!;

    }
}
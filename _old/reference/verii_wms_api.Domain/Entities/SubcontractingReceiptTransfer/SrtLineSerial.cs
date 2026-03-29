using System;
using System.Collections.Generic;

namespace WMS_WEBAPI.Models
{
    public class SrtLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual SrtLine Line { get; set; } = null!;

    }
}
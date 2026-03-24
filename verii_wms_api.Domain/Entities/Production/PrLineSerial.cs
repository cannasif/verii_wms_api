using System;

namespace WMS_WEBAPI.Models
{
    public class PrLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual PrLine Line { get; set; } = null!;
    }
}

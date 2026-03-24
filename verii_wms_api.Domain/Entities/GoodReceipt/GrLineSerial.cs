using System;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Models
{
    public class GrLineSerial : BaseLineSerialEntity
    {
        public long? LineId { get; set; }
        public virtual GrLine? Line { get; set; }
    }
}

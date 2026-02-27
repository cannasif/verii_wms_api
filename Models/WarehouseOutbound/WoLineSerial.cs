using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WO_LINE_SERIAL")]
    public class WoLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual WoLine Line { get; set; } = null!;

    }
}
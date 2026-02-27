using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WI_LINE_SERIAL")]
    public class WiLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual WiLine Line { get; set; } = null!;

    }
}
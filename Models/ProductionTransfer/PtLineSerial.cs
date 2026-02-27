using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_PT_LINE_SERIAL")]
    public class PtLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual PtLine Line { get; set; } = null!;

    }
}
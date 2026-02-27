using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WT_LINE_SERIAL")]
    public class WtLineSerial : BaseLineSerialEntity
    {
       [Required, ForeignKey(nameof(Line))]
        public long LineId { get; set; }
        public virtual WtLine Line { get; set; } = null!;

    }
}
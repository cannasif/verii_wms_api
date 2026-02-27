using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SRT_LINE_SERIAL")]
    public class SrtLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual SrtLine Line { get; set; } = null!;

    }
}
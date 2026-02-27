using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WI_IMPORT_LINE")]
    public class WiImportLine : BaseImportLineEntity
    {
        [Required, ForeignKey(nameof(Header))]
        public long HeaderId { get; set; }
        public virtual WiHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual WiLine? Line { get; set; }

    }
}

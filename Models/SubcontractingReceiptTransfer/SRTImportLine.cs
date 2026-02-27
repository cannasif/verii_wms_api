using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SRT_IMPORT_LINE")]
    public class SrtImportLine : BaseImportLineEntity
    {

        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual SrtHeader Header { get; set; } = null!;

        public long? LineId { get; set; }
        [ForeignKey(nameof(LineId))]
        public virtual SrtLine? Line { get; set; }


    }
}

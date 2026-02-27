using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_WT_IMPORT_LINE")]
    public class WtImportLine : BaseImportLineEntity
    {
        // Header ilişkisi
        [Required, ForeignKey(nameof(Header))]
        public long HeaderId { get; set; }
        public virtual WtHeader Header { get; set; } = null!;

        // Üst işlem (Line)
        [ForeignKey(nameof(Line))]
        public long? LineId { get; set; }
        public virtual WtLine Line { get; set; } = null!;

       // Navigation properties
        public virtual ICollection<WtRoute> Routes { get; set; } = new List<WtRoute>();

    }
}

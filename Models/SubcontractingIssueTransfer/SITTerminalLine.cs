using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_SIT_TERMINAL_LINE")]
    public class SitTerminalLine : BaseEntity
    { 
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual SitHeader Header { get; set; } = null!;

        
        public long TerminalUserId { get; set; }
        [ForeignKey(nameof(TerminalUserId))]
        public virtual User User { get; set; } = null!;

    }
}

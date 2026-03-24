using System;

namespace WMS_WEBAPI.Models
{
    public class SitTerminalLine : BaseEntity
    { 
        public long HeaderId { get; set; }
        public virtual SitHeader Header { get; set; } = null!;

        
        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}

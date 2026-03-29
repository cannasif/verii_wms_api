using System;

namespace WMS_WEBAPI.Models
{
    public class IcTerminalLine : BaseEntity
    {        
        public long HeaderId { get; set; }
        public virtual IcHeader Header { get; set; } = null!;
        
        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}

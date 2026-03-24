using System;

namespace WMS_WEBAPI.Models
{
    public class PtTerminalLine : BaseEntity
    {

        public long HeaderId { get; set; }
        public virtual PtHeader Header { get; set; } = null!;

        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;
        
    }
}

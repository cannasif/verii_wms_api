using System;

namespace WMS_WEBAPI.Models
{
    public class WiTerminalLine : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual WiHeader Header { get; set; } = null!;

        
        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;


    }
}
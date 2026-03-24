using System;

namespace WMS_WEBAPI.Models
{
    public class GrTerminalLine : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual GrHeader Header { get; set; } = null!;

        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

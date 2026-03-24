
namespace WMS_WEBAPI.Models
{
    public class ShTerminalLine : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual ShHeader Header { get; set; } = null!;

        public long TerminalUserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

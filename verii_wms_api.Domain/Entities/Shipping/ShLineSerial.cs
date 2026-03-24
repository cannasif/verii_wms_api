
namespace WMS_WEBAPI.Models
{
    public class ShLineSerial : BaseLineSerialEntity
    {
        public long LineId { get; set; }
        public virtual ShLine Line { get; set; } = null!;
    }
}

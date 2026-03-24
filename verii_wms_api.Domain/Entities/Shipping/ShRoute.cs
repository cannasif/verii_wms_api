
namespace WMS_WEBAPI.Models
{
    public class ShRoute : BaseRouteEntity
    {
        public long ImportLineId { get; set; }
        public virtual ShImportLine ImportLine { get; set; } = null!;
    }
}

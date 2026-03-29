using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrHeaderSerial : BaseEntity
{
    public long HeaderId { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public decimal? Amount { get; set; }
    public PrHeader? Header { get; set; }
}

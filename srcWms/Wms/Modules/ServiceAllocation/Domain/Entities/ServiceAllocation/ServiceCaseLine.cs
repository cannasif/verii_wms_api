using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;

namespace Wms.Domain.Entities.ServiceAllocation;

public sealed class ServiceCaseLine : BaseEntity
{
    public long ServiceCaseId { get; set; }
    public ServiceCase ServiceCase { get; set; } = null!;
    public ServiceCaseLineType LineType { get; set; }
    public ServiceProcessType ProcessType { get; set; } = ServiceProcessType.ServiceRepair;
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;

namespace Wms.Domain.Entities.ServiceAllocation;

public sealed class OrderAllocationLine : BaseEntity
{
    public string StockCode { get; set; } = null!;
    public long StockId { get; set; }
    public string ErpOrderNo { get; set; } = null!;
    public string ErpOrderId { get; set; } = null!;
    public string CustomerCode { get; set; } = null!;
    public long? CustomerId { get; set; }
    public ServiceProcessType ProcessType { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal AllocatedQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal FulfilledQuantity { get; set; }
    public int PriorityNo { get; set; }
    public AllocationStatus Status { get; set; } = AllocationStatus.Waiting;
    public string? SourceModule { get; set; }
    public long? SourceHeaderId { get; set; }
    public long? SourceLineId { get; set; }

    public ICollection<BusinessDocumentLink> DocumentLinks { get; set; } = new List<BusinessDocumentLink>();
}

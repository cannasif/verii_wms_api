using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;
using Wms.Domain.Common;

namespace Wms.Domain.Entities.ServiceAllocation;

public sealed class BusinessDocumentLink : BaseEntity
{
    public BusinessEntityType BusinessEntityType { get; set; }
    public long BusinessEntityId { get; set; }
    public long? ServiceCaseId { get; set; }
    public ServiceCase? ServiceCase { get; set; }
    public long? OrderAllocationLineId { get; set; }
    public OrderAllocationLine? OrderAllocationLine { get; set; }
    public DocumentModule DocumentModule { get; set; }
    public long DocumentHeaderId { get; set; }
    public long? DocumentLineId { get; set; }
    public DocumentLinkPurpose LinkPurpose { get; set; }
    public int SequenceNo { get; set; }
    public long? FromWarehouseId { get; set; }
    public long? ToWarehouseId { get; set; }
    public string? Note { get; set; }
    public DateTime LinkedAt { get; set; } = DateTimeProvider.Now;
}

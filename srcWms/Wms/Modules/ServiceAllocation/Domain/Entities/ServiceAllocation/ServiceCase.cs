using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;

namespace Wms.Domain.Entities.ServiceAllocation;

public sealed class ServiceCase : BaseEntity
{
    public string CaseNo { get; set; } = null!;
    public string CustomerCode { get; set; } = null!;
    public long? CustomerId { get; set; }
    public string? IncomingStockCode { get; set; }
    public long? IncomingStockId { get; set; }
    public string? IncomingSerialNo { get; set; }
    public long? IntakeWarehouseId { get; set; }
    public long? CurrentWarehouseId { get; set; }
    public string? DiagnosisNote { get; set; }
    public ServiceCaseStatus Status { get; set; } = ServiceCaseStatus.Draft;
    public DateTime? ReceivedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public ICollection<ServiceCaseLine> Lines { get; set; } = new List<ServiceCaseLine>();
    public ICollection<BusinessDocumentLink> DocumentLinks { get; set; } = new List<BusinessDocumentLink>();
}

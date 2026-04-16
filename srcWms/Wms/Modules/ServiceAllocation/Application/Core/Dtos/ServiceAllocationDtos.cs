using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;

namespace Wms.Application.ServiceAllocation.Dtos;

public sealed class ServiceCaseDto : BaseEntityDto
{
    public string CaseNo { get; set; } = string.Empty;
    public string CustomerCode { get; set; } = string.Empty;
    public long? CustomerId { get; set; }
    public string? IncomingStockCode { get; set; }
    public long? IncomingStockId { get; set; }
    public string? IncomingSerialNo { get; set; }
    public long? IntakeWarehouseId { get; set; }
    public long? CurrentWarehouseId { get; set; }
    public string? DiagnosisNote { get; set; }
    public ServiceCaseStatus Status { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}

public sealed class CreateServiceCaseDto
{
    [Required, StringLength(50)]
    public string CaseNo { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string CustomerCode { get; set; } = string.Empty;
    public long? CustomerId { get; set; }
    [StringLength(35)] public string? IncomingStockCode { get; set; }
    public long? IncomingStockId { get; set; }
    [StringLength(100)] public string? IncomingSerialNo { get; set; }
    public long? IntakeWarehouseId { get; set; }
    public long? CurrentWarehouseId { get; set; }
    [StringLength(1000)] public string? DiagnosisNote { get; set; }
    public ServiceCaseStatus Status { get; set; } = ServiceCaseStatus.Draft;
    public DateTime? ReceivedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string BranchCode { get; set; } = "0";
}

public sealed class UpdateServiceCaseDto
{
    [StringLength(50)] public string? CaseNo { get; set; }
    [StringLength(20)] public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    [StringLength(35)] public string? IncomingStockCode { get; set; }
    public long? IncomingStockId { get; set; }
    [StringLength(100)] public string? IncomingSerialNo { get; set; }
    public long? IntakeWarehouseId { get; set; }
    public long? CurrentWarehouseId { get; set; }
    [StringLength(1000)] public string? DiagnosisNote { get; set; }
    public ServiceCaseStatus? Status { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? BranchCode { get; set; }
}

public sealed class OrderAllocationLineDto : BaseEntityDto
{
    public string StockCode { get; set; } = string.Empty;
    public long StockId { get; set; }
    public string ErpOrderNo { get; set; } = string.Empty;
    public string ErpOrderId { get; set; } = string.Empty;
    public string CustomerCode { get; set; } = string.Empty;
    public long? CustomerId { get; set; }
    public ServiceProcessType ProcessType { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal AllocatedQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal FulfilledQuantity { get; set; }
    public int PriorityNo { get; set; }
    public AllocationStatus Status { get; set; }
    public string? SourceModule { get; set; }
    public long? SourceHeaderId { get; set; }
    public long? SourceLineId { get; set; }
}

public sealed class ServiceCaseLineDto : BaseEntityDto
{
    public long ServiceCaseId { get; set; }
    public ServiceCaseLineType LineType { get; set; }
    public ServiceProcessType ProcessType { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

public sealed class CreateServiceCaseLineDto
{
    [Required]
    public long ServiceCaseId { get; set; }
    public ServiceCaseLineType LineType { get; set; }
    public ServiceProcessType ProcessType { get; set; } = ServiceProcessType.ServiceRepair;
    [StringLength(35)] public string? StockCode { get; set; }
    public long? StockId { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal Quantity { get; set; }
    [StringLength(10)] public string? Unit { get; set; }
    [StringLength(50)] public string? ErpOrderNo { get; set; }
    [StringLength(30)] public string? ErpOrderId { get; set; }
    [StringLength(250)] public string? Description { get; set; }
    public string BranchCode { get; set; } = "0";
}

public sealed class UpdateServiceCaseLineDto
{
    public long? ServiceCaseId { get; set; }
    public ServiceCaseLineType? LineType { get; set; }
    public ServiceProcessType? ProcessType { get; set; }
    [StringLength(35)] public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public decimal? Quantity { get; set; }
    [StringLength(10)] public string? Unit { get; set; }
    [StringLength(50)] public string? ErpOrderNo { get; set; }
    [StringLength(30)] public string? ErpOrderId { get; set; }
    [StringLength(250)] public string? Description { get; set; }
    public string? BranchCode { get; set; }
}

public sealed class BusinessDocumentLinkDto : BaseEntityDto
{
    public BusinessEntityType BusinessEntityType { get; set; }
    public long BusinessEntityId { get; set; }
    public long? ServiceCaseId { get; set; }
    public long? OrderAllocationLineId { get; set; }
    public DocumentModule DocumentModule { get; set; }
    public long DocumentHeaderId { get; set; }
    public long? DocumentLineId { get; set; }
    public DocumentLinkPurpose LinkPurpose { get; set; }
    public int SequenceNo { get; set; }
    public long? FromWarehouseId { get; set; }
    public long? ToWarehouseId { get; set; }
    public string? Note { get; set; }
    public DateTime LinkedAt { get; set; }
}

public sealed class CreateBusinessDocumentLinkDto
{
    public BusinessEntityType BusinessEntityType { get; set; }
    [Required]
    public long BusinessEntityId { get; set; }
    public long? ServiceCaseId { get; set; }
    public long? OrderAllocationLineId { get; set; }
    public DocumentModule DocumentModule { get; set; }
    [Required]
    public long DocumentHeaderId { get; set; }
    public long? DocumentLineId { get; set; }
    public DocumentLinkPurpose LinkPurpose { get; set; }
    public int SequenceNo { get; set; }
    public long? FromWarehouseId { get; set; }
    public long? ToWarehouseId { get; set; }
    [StringLength(250)] public string? Note { get; set; }
    public DateTime? LinkedAt { get; set; }
    public string BranchCode { get; set; } = "0";
}

public sealed class UpdateBusinessDocumentLinkDto
{
    public BusinessEntityType? BusinessEntityType { get; set; }
    public long? BusinessEntityId { get; set; }
    public long? ServiceCaseId { get; set; }
    public long? OrderAllocationLineId { get; set; }
    public DocumentModule? DocumentModule { get; set; }
    public long? DocumentHeaderId { get; set; }
    public long? DocumentLineId { get; set; }
    public DocumentLinkPurpose? LinkPurpose { get; set; }
    public int? SequenceNo { get; set; }
    public long? FromWarehouseId { get; set; }
    public long? ToWarehouseId { get; set; }
    [StringLength(250)] public string? Note { get; set; }
    public DateTime? LinkedAt { get; set; }
    public string? BranchCode { get; set; }
}

public sealed class RecomputeAllocationRequestDto
{
    [Required]
    public long StockId { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal AvailableQuantity { get; set; }
}

public sealed class AllocationRecomputeLineResultDto
{
    public long AllocationLineId { get; set; }
    public string ErpOrderNo { get; set; } = string.Empty;
    public string ErpOrderId { get; set; } = string.Empty;
    public decimal RequestedQuantity { get; set; }
    public decimal FulfilledQuantity { get; set; }
    public decimal AllocatedQuantity { get; set; }
    public AllocationStatus Status { get; set; }
    public int PriorityNo { get; set; }
}

public sealed class AllocationRecomputeResultDto
{
    public long StockId { get; set; }
    public decimal AvailableQuantity { get; set; }
    public decimal RemainingQuantity { get; set; }
    public int ProcessedLineCount { get; set; }
    public List<AllocationRecomputeLineResultDto> Lines { get; set; } = new();
}

public sealed class ServiceCaseTimelineEventDto
{
    public long DocumentLinkId { get; set; }
    public DocumentModule DocumentModule { get; set; }
    public long DocumentHeaderId { get; set; }
    public long? DocumentLineId { get; set; }
    public DocumentLinkPurpose LinkPurpose { get; set; }
    public int SequenceNo { get; set; }
    public long? FromWarehouseId { get; set; }
    public long? ToWarehouseId { get; set; }
    public string? Note { get; set; }
    public DateTime LinkedAt { get; set; }
}

public sealed class ServiceCaseTimelineDto
{
    public ServiceCaseDto ServiceCase { get; set; } = new();
    public List<ServiceCaseLineDto> Lines { get; set; } = new();
    public List<ServiceCaseTimelineEventDto> Timeline { get; set; } = new();
}

public sealed class CreateOrderAllocationLineDto
{
    [Required, StringLength(35)]
    public string StockCode { get; set; } = string.Empty;
    [Required]
    public long? StockId { get; set; }
    [Required, StringLength(50)]
    public string ErpOrderNo { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string ErpOrderId { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string CustomerCode { get; set; } = string.Empty;
    public long? CustomerId { get; set; }
    public ServiceProcessType ProcessType { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal RequestedQuantity { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal AllocatedQuantity { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal ReservedQuantity { get; set; }
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal FulfilledQuantity { get; set; }
    public int PriorityNo { get; set; }
    public AllocationStatus Status { get; set; } = AllocationStatus.Waiting;
    [StringLength(10)] public string? SourceModule { get; set; }
    public long? SourceHeaderId { get; set; }
    public long? SourceLineId { get; set; }
    public string BranchCode { get; set; } = "0";
}

public sealed class UpdateOrderAllocationLineDto
{
    [StringLength(35)] public string? StockCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? ErpOrderNo { get; set; }
    [StringLength(30)] public string? ErpOrderId { get; set; }
    [StringLength(20)] public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public ServiceProcessType? ProcessType { get; set; }
    public decimal? RequestedQuantity { get; set; }
    public decimal? AllocatedQuantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal? FulfilledQuantity { get; set; }
    public int? PriorityNo { get; set; }
    public AllocationStatus? Status { get; set; }
    [StringLength(10)] public string? SourceModule { get; set; }
    public long? SourceHeaderId { get; set; }
    public long? SourceLineId { get; set; }
    public string? BranchCode { get; set; }
}

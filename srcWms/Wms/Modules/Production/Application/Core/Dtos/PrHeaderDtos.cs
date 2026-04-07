using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Production.Dtos;

public sealed class PrHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public decimal? Quantity { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
    public string? PlanType { get; set; }
    public string? ExecutionMode { get; set; }
    public string? Status { get; set; }
    public int Priority { get; set; }
    public string? MainStockCode { get; set; }
    public string? MainYapKod { get; set; }
    public decimal? PlannedQuantity { get; set; }
    public decimal? CompletedQuantity { get; set; }
    public DateTime? DocumentDate { get; set; }
    public string? DocumentNo { get; set; }
    public bool CanDelete { get; set; }
    public string? DeleteBlockedReason { get; set; }
}

public sealed class CreateProductionPlanRequestDto
{
    [Required]
    public string Source { get; set; } = "manual";
    [Required]
    public ProductionHeaderDraftDto Header { get; set; } = new();
    public List<ProductionOrderDraftDto> Orders { get; set; } = new();
    public List<ProductionOutputDraftDto> Outputs { get; set; } = new();
    public List<ProductionConsumptionDraftDto> Consumptions { get; set; } = new();
    public List<ProductionDependencyDraftDto> Dependencies { get; set; } = new();
}

public sealed class ProductionPlanDraftDto
{
    public string Source { get; set; } = "manual";
    public ProductionHeaderDraftDto Header { get; set; } = new();
    public List<ProductionOrderDraftDto> Orders { get; set; } = new();
    public List<ProductionOutputDraftDto> Outputs { get; set; } = new();
    public List<ProductionConsumptionDraftDto> Consumptions { get; set; } = new();
    public List<ProductionDependencyDraftDto> Dependencies { get; set; } = new();
}

public sealed class ProductionHeaderDraftDto
{
    [Required, StringLength(30)]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string ExecutionMode { get; set; } = "Serial";
    [Required, StringLength(20)]
    public string PlanType { get; set; } = "Production";
    public int Priority { get; set; }
    [StringLength(30)]
    public string ProjectCode { get; set; } = string.Empty;
    [StringLength(20)]
    public string CustomerCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string MainStockCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string MainYapKod { get; set; } = string.Empty;
    public decimal PlannedQuantity { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public List<ProductionHeaderAssignmentDraftDto> Assignments { get; set; } = new();
}

public sealed class ProductionOrderDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string OrderNo { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string OrderType { get; set; } = "Production";
    [Required, StringLength(50)]
    public string ProducedStockCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string ProducedYapKod { get; set; } = string.Empty;
    public decimal PlannedQuantity { get; set; }
    [StringLength(20)]
    public string SourceWarehouseCode { get; set; } = string.Empty;
    [StringLength(20)]
    public string TargetWarehouseCode { get; set; } = string.Empty;
    public int? SequenceNo { get; set; }
    public int? ParallelGroupNo { get; set; }
    public bool CanStartManually { get; set; }
    public bool AutoStartWhenDependenciesDone { get; set; }
    public List<ProductionOrderAssignmentDraftDto> Assignments { get; set; } = new();
}

public sealed class ProductionHeaderAssignmentDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    [Required, StringLength(20)]
    public string AssignmentType { get; set; } = "Primary";
}

public sealed class ProductionOrderAssignmentDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    [Required, StringLength(20)]
    public string AssignmentType { get; set; } = "Primary";
    [StringLength(250)]
    public string? Note { get; set; }
}

public sealed class ProductionOutputDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    [Required]
    public string OrderLocalId { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string StockCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string YapKod { get; set; } = string.Empty;
    public decimal PlannedQuantity { get; set; }
    [StringLength(20)]
    public string Unit { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string TrackingMode { get; set; } = "None";
    [Required, StringLength(20)]
    public string SerialEntryMode { get; set; } = "Optional";
    [StringLength(20)]
    public string TargetWarehouseCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string TargetCellCode { get; set; } = string.Empty;
}

public sealed class ProductionConsumptionDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    [Required]
    public string OrderLocalId { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string StockCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string YapKod { get; set; } = string.Empty;
    public decimal PlannedQuantity { get; set; }
    [StringLength(20)]
    public string Unit { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string TrackingMode { get; set; } = "None";
    [Required, StringLength(20)]
    public string SerialEntryMode { get; set; } = "Optional";
    [StringLength(20)]
    public string SourceWarehouseCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string SourceCellCode { get; set; } = string.Empty;
    public bool IsBackflush { get; set; }
    public bool IsMandatory { get; set; } = true;
}

public sealed class ProductionDependencyDraftDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    [Required]
    public string PredecessorOrderLocalId { get; set; } = string.Empty;
    [Required]
    public string SuccessorOrderLocalId { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string DependencyType { get; set; } = "FinishToStart";
    public bool RequiredTransferCompleted { get; set; }
    public bool RequiredOutputAvailable { get; set; }
    public int LagMinutes { get; set; }
}

public sealed class ProductionErpTemplateRequestDto
{
    [StringLength(30)]
    public string? OrderNo { get; set; }
    [StringLength(50)]
    public string? StockCode { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; } = 1m;
}

public sealed class PrHeaderDetailDto
{
    public PrHeaderDto Header { get; set; } = new();
    public List<PrHeaderAssignmentItemDto> HeaderAssignments { get; set; } = new();
    public List<PrOrderDetailDto> Orders { get; set; } = new();
    public List<PrDependencyDetailDto> Dependencies { get; set; } = new();
}

public sealed class PrHeaderAssignmentItemDto
{
    public long Id { get; set; }
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public string AssignmentType { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public bool IsActive { get; set; }
}

public sealed class PrOrderAssignmentItemDto
{
    public long Id { get; set; }
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public string AssignmentType { get; set; } = string.Empty;
    public string? Note { get; set; }
    public bool IsActive { get; set; }
}

public sealed class PrOrderOutputItemDto
{
    public long Id { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? ProducedQuantity { get; set; }
    public string? Unit { get; set; }
    public string TrackingMode { get; set; } = string.Empty;
    public string SerialEntryMode { get; set; } = string.Empty;
    public string? TargetWarehouseCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string Status { get; set; } = string.Empty;
}

public sealed class PrOrderConsumptionItemDto
{
    public long Id { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? ConsumedQuantity { get; set; }
    public string? Unit { get; set; }
    public string TrackingMode { get; set; } = string.Empty;
    public string SerialEntryMode { get; set; } = string.Empty;
    public string? SourceWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public bool IsBackflush { get; set; }
    public bool IsMandatory { get; set; }
    public string Status { get; set; } = string.Empty;
}

public sealed class PrOrderDetailDto
{
    public long Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string OrderType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int? SequenceNo { get; set; }
    public int? ParallelGroupNo { get; set; }
    public string ProducedStockCode { get; set; } = string.Empty;
    public string? ProducedYapKod { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? StartedQuantity { get; set; }
    public decimal? CompletedQuantity { get; set; }
    public decimal? ScrapQuantity { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public bool CanStartManually { get; set; }
    public bool AutoStartWhenDependenciesDone { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public List<PrOrderAssignmentItemDto> Assignments { get; set; } = new();
    public List<PrOrderOutputItemDto> Outputs { get; set; } = new();
    public List<PrOrderConsumptionItemDto> Consumptions { get; set; } = new();
}

public sealed class PrDependencyDetailDto
{
    public long Id { get; set; }
    public long PredecessorOrderId { get; set; }
    public string PredecessorOrderNo { get; set; } = string.Empty;
    public long SuccessorOrderId { get; set; }
    public string SuccessorOrderNo { get; set; } = string.Empty;
    public string DependencyType { get; set; } = string.Empty;
    public bool RequiredTransferCompleted { get; set; }
    public bool RequiredOutputAvailable { get; set; }
    public int LagMinutes { get; set; }
}

public sealed class CreatePrHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(50)]
    public string? StockCode { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public decimal? Quantity { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdatePrHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(50)]
    public string? StockCode { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public decimal? Quantity { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class PrAssignedProductionOrderLinesDto
{
    public IEnumerable<PrLineDto> Lines { get; set; } = Array.Empty<PrLineDto>();
    public IEnumerable<PrLineSerialDto> LineSerials { get; set; } = Array.Empty<PrLineSerialDto>();
    public IEnumerable<PrImportLineDto> ImportLines { get; set; } = Array.Empty<PrImportLineDto>();
    public IEnumerable<PrRouteDto> Routes { get; set; } = Array.Empty<PrRouteDto>();
}

public sealed class CreatePrLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreatePrLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreatePrRouteWithLineKeyDto : BaseRouteCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? ImportLineClientKey { get; set; }
    public Guid? ImportLineGroupGuid { get; set; }
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
}

public sealed class CreatePrImportLineWithKeysDto : BaseImportLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
}

public sealed class CreatePrTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateProductionOrderRequestDto
{
    [Required]
    public CreatePrHeaderDto Header { get; set; } = null!;
    public List<CreatePrLineWithKeyDto>? Lines { get; set; }
    public List<CreatePrLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePrTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkPrGenerateRequestDto
{
    [Required]
    public CreatePrHeaderDto Header { get; set; } = null!;
    public List<CreatePrLineWithKeyDto>? Lines { get; set; }
    public List<CreatePrLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePrRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreatePrImportLineWithKeysDto>? ImportLines { get; set; }
}

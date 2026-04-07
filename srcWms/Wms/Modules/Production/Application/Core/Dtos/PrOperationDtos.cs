using System.ComponentModel.DataAnnotations;

namespace Wms.Application.Production.Dtos;

public sealed class PrOperationDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string? OrderNo { get; set; }
    public string? OperationNo { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? PlannedDurationMinutes { get; set; }
    public int? ActualDurationMinutes { get; set; }
    public int? PauseDurationMinutes { get; set; }
    public int? NetWorkingDurationMinutes { get; set; }
    public string? Description { get; set; }
    public List<PrOperationEventDto> Events { get; set; } = new();
    public List<PrOperationLineDto> Lines { get; set; } = new();
}

public sealed class PrOperationEventDto
{
    public long Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? EventReasonCode { get; set; }
    public string? EventNote { get; set; }
    public DateTime EventAt { get; set; }
    public int? DurationMinutes { get; set; }
    public long? PerformedByUserId { get; set; }
    public long? WorkcenterId { get; set; }
    public long? MachineId { get; set; }
}

public sealed class PrOperationLineDto
{
    public long Id { get; set; }
    public string LineRole { get; set; } = string.Empty;
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? SerialNo1 { get; set; }
    public string? LotNo { get; set; }
    public string? BatchNo { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string? ScannedBarcode { get; set; }
    public DateTime CreatedDate { get; set; }
}

public sealed class StartPrOperationRequestDto
{
    [Required]
    public long OrderId { get; set; }
    [StringLength(20)]
    public string OperationType { get; set; } = "ProductionRun";
    public long? WorkcenterId { get; set; }
    public long? MachineId { get; set; }
    public int? PlannedDurationMinutes { get; set; }
    [StringLength(500)]
    public string? Description { get; set; }
}

public sealed class PrOperationEventRequestDto
{
    [StringLength(50)]
    public string? ReasonCode { get; set; }
    [StringLength(500)]
    public string? Note { get; set; }
    public int? DurationMinutes { get; set; }
    public long? WorkcenterId { get; set; }
    public long? MachineId { get; set; }
}

public sealed class AddPrOperationLineRequestDto
{
    [Required]
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? SerialNo1 { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? LotNo { get; set; }
    public string? BatchNo { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string? ScannedBarcode { get; set; }
}

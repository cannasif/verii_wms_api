using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.InventoryCount.Dtos;

public sealed class IcHeaderDto : BaseHeaderEntityDto
{
    public string CountType { get; set; } = string.Empty;
    public string ScopeMode { get; set; } = string.Empty;
    public string CountMode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string FreezeMode { get; set; } = string.Empty;
    public string? CellCode { get; set; }
    public string? RackCode { get; set; }
    public string? WarehouseCode { get; set; }
    public long? WarehouseId { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public bool IsFirstCount { get; set; }
    public bool RequiresRecount { get; set; }
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public int? SequenceNo { get; set; }
    public int? LineCount { get; set; }
    public int? CountedLineCount { get; set; }
    public int? DifferenceLineCount { get; set; }
    public int? RecountRequiredLineCount { get; set; }
}

public sealed class CreateIcHeaderDto : BaseHeaderCreateDto
{
    [Required, StringLength(20)] public string CountType { get; set; } = "General";
    [Required, StringLength(20)] public string ScopeMode { get; set; } = "All";
    [Required, StringLength(20)] public string CountMode { get; set; } = "Blind";
    [Required, StringLength(20)] public string FreezeMode { get; set; } = "None";
    [StringLength(35)] public string? CellCode { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(50)] public string? StockCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public bool IsFirstCount { get; set; } = true;
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public int? SequenceNo { get; set; }
}

public sealed class UpdateIcHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)] public string? CountType { get; set; }
    [StringLength(20)] public string? ScopeMode { get; set; }
    [StringLength(20)] public string? CountMode { get; set; }
    [StringLength(20)] public string? Status { get; set; }
    [StringLength(20)] public string? FreezeMode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(50)] public string? StockCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public bool? IsFirstCount { get; set; }
    public bool? RequiresRecount { get; set; }
    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }
    public int? SequenceNo { get; set; }
    public int? LineCount { get; set; }
    public int? CountedLineCount { get; set; }
    public int? DifferenceLineCount { get; set; }
    public int? RecountRequiredLineCount { get; set; }
}

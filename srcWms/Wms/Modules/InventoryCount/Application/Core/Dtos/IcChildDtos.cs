using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.InventoryCount.Dtos;

public sealed class IcImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
}

public sealed class IcImportLineWithRoutesDto
{
    public IcImportLineDto ImportLine { get; set; } = null!;
    public List<IcRouteDto> Routes { get; set; } = new();
}

public sealed class CreateIcImportLineDto : BaseImportLineCreateDto
{
    [Required] public long HeaderId { get; set; }
}

public sealed class UpdateIcImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
}

public sealed class IcRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string YapKod { get; set; } = string.Empty;
    public int Priority { get; set; }
}

public sealed class CreateIcRouteDto : BaseRouteCreateDto
{
    [Required] public long ImportLineId { get; set; }
    [StringLength(30)] public string YapKod { get; set; } = string.Empty;
    public int Priority { get; set; }
    [StringLength(100)] public string? Description { get; set; }
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    [StringLength(50)] public string? Barcode { get; set; }
    public int? OldWarehouse { get; set; }
    public int? NewWarehouse { get; set; }
}

public sealed class UpdateIcRouteDto
{
    public long? ImportLineId { get; set; }
    [StringLength(30)] public string? YapKod { get; set; }
    public int? Priority { get; set; }
    [StringLength(100)] public string? Description { get; set; }
    public decimal? OldQuantity { get; set; }
    public decimal? NewQuantity { get; set; }
    [StringLength(50)] public string? Barcode { get; set; }
    public int? OldWarehouse { get; set; }
    public int? NewWarehouse { get; set; }
}

public sealed class IcTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateIcTerminalLineDto : BaseTerminalLineCreateDto
{
    [Required] public long HeaderId { get; set; }
}

public sealed class UpdateIcTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

public sealed class IcScopeDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public int? SequenceNo { get; set; }
    public string ScopeType { get; set; } = string.Empty;
    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public long? StockId { get; set; }
    public string? StockCode { get; set; }
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }
    public bool IsActive { get; set; }
}

public sealed class CreateIcScopeDto
{
    [Required] public long HeaderId { get; set; }
    public int? SequenceNo { get; set; }
    [Required, StringLength(20)] public string ScopeType { get; set; } = "Warehouse";
    public long? WarehouseId { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? StockCode { get; set; }
    public long? YapKodId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class UpdateIcScopeDto
{
    public int? SequenceNo { get; set; }
    [StringLength(20)] public string? ScopeType { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? StockCode { get; set; }
    public long? YapKodId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    public bool? IsActive { get; set; }
}

public sealed class IcLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long? ScopeId { get; set; }
    public int SequenceNo { get; set; }
    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }
    public long StockId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public string? LotNo { get; set; }
    public string? SerialNo1 { get; set; }
    public string? SerialNo2 { get; set; }
    public string? BatchNo { get; set; }
    public string? Unit { get; set; }
    public decimal ExpectedQuantity { get; set; }
    public decimal? CountedQuantity { get; set; }
    public decimal? DifferenceQuantity { get; set; }
    public string CountStatus { get; set; } = string.Empty;
    public int EntryCount { get; set; }
    public bool IsMatched { get; set; }
    public bool IsDifference { get; set; }
    public bool IsExtraStock { get; set; }
    public bool IsMissingStock { get; set; }
    public bool IsRecountRequired { get; set; }
    public DateTime? FirstCountedAt { get; set; }
    public DateTime? LastCountedAt { get; set; }
    public long? CountedByUserId { get; set; }
    public string? ApprovalNote { get; set; }
    public string? DifferenceReason { get; set; }
}

public sealed class CreateIcLineDto
{
    [Required] public long HeaderId { get; set; }
    public long? ScopeId { get; set; }
    public int SequenceNo { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    public long? StockId { get; set; }
    [Required, StringLength(50)] public string StockCode { get; set; } = string.Empty;
    public long? YapKodId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    [StringLength(50)] public string? LotNo { get; set; }
    [StringLength(100)] public string? SerialNo1 { get; set; }
    [StringLength(100)] public string? SerialNo2 { get; set; }
    [StringLength(50)] public string? BatchNo { get; set; }
    [StringLength(20)] public string? Unit { get; set; }
    public decimal ExpectedQuantity { get; set; }
}

public sealed class UpdateIcLineDto
{
    public int? SequenceNo { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    public long? StockId { get; set; }
    [StringLength(50)] public string? StockCode { get; set; }
    public long? YapKodId { get; set; }
    [StringLength(50)] public string? YapKod { get; set; }
    [StringLength(50)] public string? LotNo { get; set; }
    [StringLength(100)] public string? SerialNo1 { get; set; }
    [StringLength(100)] public string? SerialNo2 { get; set; }
    [StringLength(50)] public string? BatchNo { get; set; }
    [StringLength(20)] public string? Unit { get; set; }
    public decimal? ExpectedQuantity { get; set; }
    public string? ApprovalNote { get; set; }
    public string? DifferenceReason { get; set; }
}

public sealed class IcCountEntryDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long LineId { get; set; }
    public int EntryNo { get; set; }
    public string EntryType { get; set; } = string.Empty;
    public decimal EnteredQuantity { get; set; }
    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }
    public DateTime EnteredAt { get; set; }
    public long? EnteredByUserId { get; set; }
    public string? DeviceCode { get; set; }
    public string? Note { get; set; }
}

public sealed class CreateIcCountEntryDto
{
    [Required] public long LineId { get; set; }
    [StringLength(20)] public string EntryType { get; set; } = "FirstCount";
    [Required] public decimal EnteredQuantity { get; set; }
    public long? WarehouseId { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    [StringLength(35)] public string? RackCode { get; set; }
    [StringLength(35)] public string? CellCode { get; set; }
    [StringLength(50)] public string? DeviceCode { get; set; }
    [StringLength(250)] public string? Note { get; set; }
}

public sealed class IcAdjustmentDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long LineId { get; set; }
    public decimal ExpectedQuantity { get; set; }
    public decimal ApprovedCountedQuantity { get; set; }
    public decimal DifferenceQuantity { get; set; }
    public string AdjustmentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ErpReferenceNo { get; set; }
    public DateTime? PostingDate { get; set; }
    public string? Note { get; set; }
}

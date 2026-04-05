using System.ComponentModel.DataAnnotations;

namespace Wms.Application.Common;

public class BaseHeaderEntityDto : BaseEntityDto
{
    public string YearCode { get; set; } = string.Empty;
    public string? ProjectCode { get; set; }
    public long? CustomerId { get; set; }
    public long? SourceWarehouseId { get; set; }
    public long? TargetWarehouseId { get; set; }
    public string? OrderId { get; set; }
    public DateTime PlannedDate { get; set; }
    public bool IsPlanned { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public byte? PriorityLevel { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPendingApproval { get; set; }
    public bool? ApprovalStatus { get; set; }
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public bool IsERPIntegrated { get; set; }
    public string? ERPReferenceNumber { get; set; }
    public DateTime? ERPIntegrationDate { get; set; }
    public string? ERPIntegrationStatus { get; set; }
    public string? ERPErrorMessage { get; set; }
}

public class BaseHeaderCreateDto
{
    public string BranchCode { get; set; } = string.Empty;
    public string? ProjectCode { get; set; }
    public long? CustomerId { get; set; }
    public long? SourceWarehouseId { get; set; }
    public long? TargetWarehouseId { get; set; }
    public string? OrderId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string YearCode { get; set; } = string.Empty;
    [StringLength(100)]
    public string? Description1 { get; set; }
    [StringLength(100)]
    public string? Description2 { get; set; }
    public byte? PriorityLevel { get; set; }
    public DateTime PlannedDate { get; set; }
    public bool IsPlanned { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public class BaseHeaderUpdateDto
{
    public string? BranchCode { get; set; }
    public string? ProjectCode { get; set; }
    public long? CustomerId { get; set; }
    public long? SourceWarehouseId { get; set; }
    public long? TargetWarehouseId { get; set; }
    public string? OrderId { get; set; }
    public string? DocumentType { get; set; }
    public string? YearCode { get; set; }
    [StringLength(100)]
    public string? Description1 { get; set; }
    [StringLength(100)]
    public string? Description2 { get; set; }
    public byte? PriorityLevel { get; set; }
    public DateTime? PlannedDate { get; set; }
    public bool? IsPlanned { get; set; }
}

public class BaseLineEntityDto : BaseEntityDto
{
    public string StockCode { get; set; } = string.Empty;
    public long? StockId { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? YapAcik { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

public class BaseLineCreateDto
{
    public string StockCode { get; set; } = string.Empty;
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

public class BaseLineUpdateDto
{
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public decimal? Quantity { get; set; }
    public string? Unit { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderId { get; set; }
    public string? Description { get; set; }
}

public class BaseImportLineEntityDto : BaseEntityDto
{
    public string StockCode { get; set; } = string.Empty;
    public long? StockId { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? YapAcik { get; set; }
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description { get; set; }
}

public class BaseImportLineCreateDto
{
    public string StockCode { get; set; } = string.Empty;
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description { get; set; }
}

public class BaseImportLineUpdateDto
{
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? YapAcik { get; set; }
    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description { get; set; }
}

public class BaseRouteEntityDto : BaseEntityDto
{
    public string ScannedBarcode { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public int? SourceWarehouse { get; set; }
    public int? TargetWarehouse { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public class BaseRouteCreateDto
{
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? ScannedBarcode { get; set; }
    public int? SourceWarehouse { get; set; }
    public int? TargetWarehouse { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public class BaseLineSerialEntityDto : BaseEntityDto
{
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public class BaseLineSerialCreateDto
{
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public class BaseLineSerialUpdateDto
{
    public decimal? Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public class BaseTerminalLineCreateDto
{
    public long TerminalUserId { get; set; }
}

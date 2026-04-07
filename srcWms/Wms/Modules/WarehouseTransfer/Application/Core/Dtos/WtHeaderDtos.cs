using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.WarehouseTransfer.Dtos;

public sealed class WtHeaderDto : BaseHeaderEntityDto
{
    [Required]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
    public string? Priority { get; set; }
    public byte? Type { get; set; }
}

public sealed class CreateWtHeaderDto : BaseHeaderCreateDto
{
    [StringLength(50)]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
    [StringLength(10)]
    public string? Priority { get; set; }
    [Required]
    public byte Type { get; set; }
}

public sealed class UpdateWtHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(50)]
    public string? DocumentNo { get; set; }
    public DateTime? DocumentDate { get; set; }
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
    [StringLength(10)]
    public string? Priority { get; set; }
    public byte? Type { get; set; }
}

public sealed class WtAssignedOrderLinesDto
{
    public IEnumerable<WtLineDto> Lines { get; set; } = Array.Empty<WtLineDto>();
    public IEnumerable<WtLineSerialDto> LineSerials { get; set; } = Array.Empty<WtLineSerialDto>();
    public IEnumerable<WtImportLineDto> ImportLines { get; set; } = Array.Empty<WtImportLineDto>();
    public IEnumerable<WtRouteDto> Routes { get; set; } = Array.Empty<WtRouteDto>();
}

public sealed class CreateWtLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateWtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateWtRouteWithLineKeyDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? ImportLineClientKey { get; set; }
    public Guid? ImportLineGroupGuid { get; set; }
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? ScannedBarcode { get; set; }
    public long? SourceWarehouse { get; set; }
    public long? TargetWarehouse { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string? Description { get; set; }
}

public sealed class CreateWtImportLineWithKeysDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
    public long? StockId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
}

public sealed class CreateWtTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateWarehouseTransferOrderRequestDto
{
    [Required]
    public CreateWtHeaderDto Header { get; set; } = null!;
    public List<CreateWtLineWithKeyDto>? Lines { get; set; }
    public List<CreateWtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWtTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkCreateWtRequestDto
{
    [Required]
    public CreateWtHeaderDto Header { get; set; } = null!;
    public List<CreateWtLineWithKeyDto>? Lines { get; set; }
    public List<CreateWtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWtRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateWtImportLineWithKeysDto>? ImportLines { get; set; }
}

public sealed class ProcessWtRouteDto
{
    public long? StockId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? ScannedBarcode { get; set; }
    public long? SourceWarehouse { get; set; }
    public long? TargetWarehouse { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public sealed class ProcessWtRequestDto
{
    [Required]
    public CreateWtHeaderDto Header { get; set; } = null!;
    public List<ProcessWtRouteDto>? Routes { get; set; }
}

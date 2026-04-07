using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.WarehouseOutbound.Dtos;

public sealed class WoHeaderDto : BaseHeaderEntityDto
{
    [Required]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    public string OutboundType { get; set; } = string.Empty;
    public string? AccountCode { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
    public byte Type { get; set; }
}

public sealed class CreateWoHeaderDto : BaseHeaderCreateDto
{
    [StringLength(50)]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    [Required]
    [StringLength(10)]
    public string OutboundType { get; set; } = string.Empty;
    [StringLength(20)]
    public string? AccountCode { get; set; }
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
    [Required]
    public byte Type { get; set; }
}

public sealed class UpdateWoHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(50)]
    public string? DocumentNo { get; set; }
    public DateTime? DocumentDate { get; set; }
    [StringLength(10)]
    public string? OutboundType { get; set; }
    [StringLength(20)]
    public string? AccountCode { get; set; }
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
    public byte? Type { get; set; }
}

public sealed class WoAssignedOrderLinesDto
{
    public IEnumerable<WoLineDto> Lines { get; set; } = Array.Empty<WoLineDto>();
    public IEnumerable<WoLineSerialDto> LineSerials { get; set; } = Array.Empty<WoLineSerialDto>();
    public IEnumerable<WoImportLineDto> ImportLines { get; set; } = Array.Empty<WoImportLineDto>();
    public IEnumerable<WoRouteDto> Routes { get; set; } = Array.Empty<WoRouteDto>();
}

public sealed class CreateWoLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateWoLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateWoRouteWithLineKeyDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? ImportLineClientKey { get; set; }
    public Guid? ImportLineGroupGuid { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public long? StockId { get; set; }
    public long? YapKodId { get; set; }
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

public sealed class CreateWoImportLineWithKeysDto
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

public sealed class CreateWoTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateWarehouseOutboundOrderRequestDto
{
    [Required]
    public CreateWoHeaderDto Header { get; set; } = null!;
    public List<CreateWoLineWithKeyDto>? Lines { get; set; }
    public List<CreateWoLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWoTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkCreateWoRequestDto
{
    [Required]
    public CreateWoHeaderDto Header { get; set; } = null!;
    public List<CreateWoLineWithKeyDto>? Lines { get; set; }
    public List<CreateWoLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWoRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateWoImportLineWithKeysDto>? ImportLines { get; set; }
}

public sealed class ProcessWoRouteDto
{
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public long? StockId { get; set; }
    public long? YapKodId { get; set; }
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

public sealed class ProcessWoRequestDto
{
    [Required]
    public CreateWoHeaderDto Header { get; set; } = null!;
    public List<ProcessWoRouteDto>? Routes { get; set; }
}

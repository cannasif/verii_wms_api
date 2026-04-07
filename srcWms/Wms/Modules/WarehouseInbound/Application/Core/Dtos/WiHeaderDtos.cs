using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.WarehouseInbound.Dtos;

public sealed class WiHeaderDto : BaseHeaderEntityDto
{
    [Required]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    public string InboundType { get; set; } = string.Empty;
    public string? AccountCode { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
    public byte Type { get; set; }
}

public sealed class CreateWiHeaderDto : BaseHeaderCreateDto
{
    [StringLength(50)]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    [Required]
    [StringLength(10)]
    public string InboundType { get; set; } = string.Empty;
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

public sealed class UpdateWiHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(50)]
    public string? DocumentNo { get; set; }
    public DateTime? DocumentDate { get; set; }
    [StringLength(10)]
    public string? InboundType { get; set; }
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

public sealed class WiAssignedOrderLinesDto
{
    public IEnumerable<WiLineDto> Lines { get; set; } = Array.Empty<WiLineDto>();
    public IEnumerable<WiLineSerialDto> LineSerials { get; set; } = Array.Empty<WiLineSerialDto>();
    public IEnumerable<WiImportLineDto> ImportLines { get; set; } = Array.Empty<WiImportLineDto>();
    public IEnumerable<WiRouteDto> Routes { get; set; } = Array.Empty<WiRouteDto>();
}

public sealed class CreateWiLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateWiLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateWiRouteWithLineKeyDto
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

public sealed class CreateWiImportLineWithKeysDto
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

public sealed class CreateWiTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateWarehouseInboundOrderRequestDto
{
    [Required]
    public CreateWiHeaderDto Header { get; set; } = null!;
    public List<CreateWiLineWithKeyDto>? Lines { get; set; }
    public List<CreateWiLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWiTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkCreateWiRequestDto
{
    [Required]
    public CreateWiHeaderDto Header { get; set; } = null!;
    public List<CreateWiLineWithKeyDto>? Lines { get; set; }
    public List<CreateWiLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateWiRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateWiImportLineWithKeysDto>? ImportLines { get; set; }
}

public sealed class ProcessWiRouteDto
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

public sealed class ProcessWiRequestDto
{
    [Required]
    public CreateWiHeaderDto Header { get; set; } = null!;
    public List<ProcessWiRouteDto>? Routes { get; set; }
}

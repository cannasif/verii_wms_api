using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.ProductionTransfer.Dtos;

public sealed class PtHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
}

public sealed class CreatePtHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdatePtHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class PtAssignedProductionTransferOrderLinesDto
{
    public IEnumerable<PtLineDto> Lines { get; set; } = Array.Empty<PtLineDto>();
    public IEnumerable<PtLineSerialDto> LineSerials { get; set; } = Array.Empty<PtLineSerialDto>();
    public IEnumerable<PtImportLineDto> ImportLines { get; set; } = Array.Empty<PtImportLineDto>();
    public IEnumerable<PtRouteDto> Routes { get; set; } = Array.Empty<PtRouteDto>();
}

public sealed class CreatePtLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreatePtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreatePtRouteWithLineKeyDto : BaseRouteCreateDto
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

public sealed class CreatePtImportLineWithKeysDto : BaseImportLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? ScannedBarkod { get; set; }
    public string? ErpOrderNumber { get; set; }
    public string? ErpOrderNo { get; set; }
    public string? ErpOrderLineNumber { get; set; }
}

public sealed class CreatePtTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateProductionTransferOrderRequestDto
{
    [Required]
    public CreatePtHeaderDto Header { get; set; } = null!;
    public List<CreatePtLineWithKeyDto>? Lines { get; set; }
    public List<CreatePtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePtTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkPtGenerateRequestDto
{
    [Required]
    public CreatePtHeaderDto Header { get; set; } = null!;
    public List<CreatePtLineWithKeyDto>? Lines { get; set; }
    public List<CreatePtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePtRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreatePtImportLineWithKeysDto>? ImportLines { get; set; }
}

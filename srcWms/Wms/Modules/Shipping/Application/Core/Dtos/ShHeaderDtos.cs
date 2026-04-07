using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Shipping.Dtos;

public sealed class ShHeaderDto : BaseHeaderEntityDto
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
    public byte Type { get; set; }
}

public sealed class CreateShHeaderDto : BaseHeaderCreateDto
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
    [Required]
    public byte Type { get; set; }
}

public sealed class UpdateShHeaderDto : BaseHeaderUpdateDto
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
    public byte? Type { get; set; }
}

public sealed class ShAssignedOrderLinesDto
{
    public IEnumerable<ShLineDto> Lines { get; set; } = Array.Empty<ShLineDto>();
    public IEnumerable<ShLineSerialDto> LineSerials { get; set; } = Array.Empty<ShLineSerialDto>();
    public IEnumerable<ShImportLineDto> ImportLines { get; set; } = Array.Empty<ShImportLineDto>();
    public IEnumerable<ShRouteDto> Routes { get; set; } = Array.Empty<ShRouteDto>();
}

public sealed class CreateShLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateShLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateShRouteWithLineKeyDto : BaseRouteCreateDto
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

public sealed class CreateShImportLineWithKeysDto : BaseImportLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
}

public sealed class CreateShTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateShipmentOrderRequestDto
{
    [Required]
    public CreateShHeaderDto Header { get; set; } = null!;
    public List<CreateShLineWithKeyDto>? Lines { get; set; }
    public List<CreateShLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateShTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkCreateShRequestDto
{
    [Required]
    public CreateShHeaderDto Header { get; set; } = null!;
    public List<CreateShLineWithKeyDto>? Lines { get; set; }
    public List<CreateShLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateShRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateShImportLineWithKeysDto>? ImportLines { get; set; }
}

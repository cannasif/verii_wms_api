using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.SubcontractingReceiptTransfer.Dtos;

public sealed class SrtHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
}

public sealed class CreateSrtHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdateSrtHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class SrtAssignedSubcontractingReceiptTransferOrderLinesDto
{
    public IEnumerable<SrtLineDto> Lines { get; set; } = Array.Empty<SrtLineDto>();
    public IEnumerable<SrtLineSerialDto> LineSerials { get; set; } = Array.Empty<SrtLineSerialDto>();
    public IEnumerable<SrtImportLineDto> ImportLines { get; set; } = Array.Empty<SrtImportLineDto>();
    public IEnumerable<SrtRouteDto> Routes { get; set; } = Array.Empty<SrtRouteDto>();
}

public sealed class CreateSrtLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateSrtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateSrtRouteWithLineKeyDto : BaseRouteCreateDto
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

public sealed class CreateSrtImportLineWithKeysDto : BaseImportLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
}

public sealed class CreateSrtTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateSubcontractingReceiptTransferOrderRequestDto
{
    [Required]
    public CreateSrtHeaderDto Header { get; set; } = null!;
    public List<CreateSrtLineWithKeyDto>? Lines { get; set; }
    public List<CreateSrtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateSrtTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkSrtGenerateRequestDto
{
    [Required]
    public CreateSrtHeaderDto Header { get; set; } = null!;
    public List<CreateSrtLineWithKeyDto>? Lines { get; set; }
    public List<CreateSrtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateSrtRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateSrtImportLineWithKeysDto>? ImportLines { get; set; }
}

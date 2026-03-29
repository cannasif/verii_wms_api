using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.SubcontractingIssueTransfer.Dtos;

public sealed class SitHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
}

public sealed class CreateSitHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdateSitHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class SitAssignedSubcontractingIssueTransferOrderLinesDto
{
    public IEnumerable<SitLineDto> Lines { get; set; } = Array.Empty<SitLineDto>();
    public IEnumerable<SitLineSerialDto> LineSerials { get; set; } = Array.Empty<SitLineSerialDto>();
    public IEnumerable<SitImportLineDto> ImportLines { get; set; } = Array.Empty<SitImportLineDto>();
    public IEnumerable<SitRouteDto> Routes { get; set; } = Array.Empty<SitRouteDto>();
}

public sealed class CreateSitLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateSitLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateSitRouteWithLineKeyDto : BaseRouteCreateDto
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

public sealed class CreateSitImportLineWithKeysDto : BaseImportLineCreateDto
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

public sealed class CreateSitTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateSubcontractingIssueTransferOrderRequestDto
{
    [Required]
    public CreateSitHeaderDto Header { get; set; } = null!;
    public List<CreateSitLineWithKeyDto>? Lines { get; set; }
    public List<CreateSitLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateSitTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkSitGenerateRequestDto
{
    [Required]
    public CreateSitHeaderDto Header { get; set; } = null!;
    public List<CreateSitLineWithKeyDto>? Lines { get; set; }
    public List<CreateSitLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateSitRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreateSitImportLineWithKeysDto>? ImportLines { get; set; }
}

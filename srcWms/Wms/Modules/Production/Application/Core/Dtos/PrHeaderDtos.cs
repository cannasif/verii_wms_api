using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Production.Dtos;

public sealed class PrHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public decimal? Quantity { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
}

public sealed class CreatePrHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(50)]
    public string? StockCode { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public decimal? Quantity { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdatePrHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(50)]
    public string? StockCode { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public decimal? Quantity { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class PrAssignedProductionOrderLinesDto
{
    public IEnumerable<PrLineDto> Lines { get; set; } = Array.Empty<PrLineDto>();
    public IEnumerable<PrLineSerialDto> LineSerials { get; set; } = Array.Empty<PrLineSerialDto>();
    public IEnumerable<PrImportLineDto> ImportLines { get; set; } = Array.Empty<PrImportLineDto>();
    public IEnumerable<PrRouteDto> Routes { get; set; } = Array.Empty<PrRouteDto>();
}

public sealed class CreatePrLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreatePrLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreatePrRouteWithLineKeyDto : BaseRouteCreateDto
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

public sealed class CreatePrImportLineWithKeysDto : BaseImportLineCreateDto
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

public sealed class CreatePrTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateProductionOrderRequestDto
{
    [Required]
    public CreatePrHeaderDto Header { get; set; } = null!;
    public List<CreatePrLineWithKeyDto>? Lines { get; set; }
    public List<CreatePrLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePrTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkPrGenerateRequestDto
{
    [Required]
    public CreatePrHeaderDto Header { get; set; } = null!;
    public List<CreatePrLineWithKeyDto>? Lines { get; set; }
    public List<CreatePrLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePrRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreatePrImportLineWithKeysDto>? ImportLines { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Shipping.Dtos;

public sealed class ShLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateShLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class UpdateShLineDto : BaseLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class ShImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class ShImportLineWithRoutesDto
{
    public ShImportLineDto ImportLine { get; set; } = null!;
    public List<ShRouteDto> Routes { get; set; } = new();
}

public sealed class CreateShImportLineDto : BaseImportLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class UpdateShImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class AddShImportBarcodeRequestDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    [Required]
    [StringLength(75)]
    public string Barcode { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    [StringLength(50)]
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    [Required]
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public sealed class ShRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreateShRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateShRouteDto
{
    public long? ImportLineId { get; set; }
    public string? ScannedBarcode { get; set; }
    public decimal? Quantity { get; set; }
    public string? StockCode { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public long? SourceWarehouse { get; set; }
    public long? TargetWarehouse { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public sealed class ShLineSerialDto : BaseLineSerialEntityDto
{
    public long LineId { get; set; }
}

public sealed class CreateShLineSerialDto : BaseLineSerialCreateDto
{
    [Required]
    public long LineId { get; set; }
}

public sealed class UpdateShLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
}


public sealed class ShTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateShTerminalLineDto : BaseTerminalLineCreateDto
{
}

public sealed class UpdateShTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

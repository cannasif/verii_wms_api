using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Production.Dtos;

public sealed class PrLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreatePrLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class UpdatePrLineDto : BaseLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class PrImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class PrImportLineWithRoutesDto
{
    public PrImportLineDto ImportLine { get; set; } = null!;
    public List<PrRouteDto> Routes { get; set; } = new();
}

public sealed class CreatePrImportLineDto : BaseImportLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class UpdatePrImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class AddPrImportBarcodeRequestDto
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

public sealed class PrRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreatePrRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdatePrRouteDto
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

public sealed class PrLineSerialDto : BaseLineSerialEntityDto
{
    public long LineId { get; set; }
}

public sealed class CreatePrLineSerialDto : BaseLineSerialCreateDto
{
    [Required]
    public long LineId { get; set; }
}

public sealed class UpdatePrLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
}


public sealed class PrTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreatePrTerminalLineDto : BaseTerminalLineCreateDto
{
}

public sealed class UpdatePrTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

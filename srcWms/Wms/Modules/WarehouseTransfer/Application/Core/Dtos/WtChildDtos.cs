using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.WarehouseTransfer.Dtos;

public sealed class WtLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateWtLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class UpdateWtLineDto : BaseLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class WtImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class WtImportLineWithRoutesDto
{
    public WtImportLineDto ImportLine { get; set; } = null!;
    public List<WtRouteDto> Routes { get; set; } = new();
}

public sealed class CreateWtImportLineDto : BaseImportLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class UpdateWtImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class AddWtImportBarcodeRequestDto
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

public sealed class WtRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreateWtRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateWtRouteDto
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

public sealed class WtLineSerialDto : BaseLineSerialEntityDto
{
    public long LineId { get; set; }
}

public sealed class CreateWtLineSerialDto : BaseLineSerialCreateDto
{
    [Required]
    public long LineId { get; set; }
}

public sealed class UpdateWtLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
}


public sealed class WtTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateWtTerminalLineDto : BaseTerminalLineCreateDto
{
}

public sealed class UpdateWtTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

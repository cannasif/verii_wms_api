using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.SubcontractingReceiptTransfer.Dtos;

public sealed class SrtLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateSrtLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class UpdateSrtLineDto : BaseLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class SrtImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class SrtImportLineWithRoutesDto
{
    public SrtImportLineDto ImportLine { get; set; } = null!;
    public List<SrtRouteDto> Routes { get; set; } = new();
}

public sealed class CreateSrtImportLineDto : BaseImportLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class UpdateSrtImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class AddSrtImportBarcodeRequestDto
{
    [Required]
    public long HeaderId { get; set; }
    [Required]
    [StringLength(75)]
    public string Barcode { get; set; } = string.Empty;
    [StringLength(50)]
    public string? StockCode { get; set; }
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

public sealed class SrtRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreateSrtRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateSrtRouteDto
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

public sealed class SrtLineSerialDto : BaseLineSerialEntityDto
{
    public long LineId { get; set; }
}

public sealed class CreateSrtLineSerialDto : BaseLineSerialCreateDto
{
    [Required]
    public long LineId { get; set; }
}

public sealed class UpdateSrtLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
}


public sealed class SrtTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateSrtTerminalLineDto : BaseTerminalLineCreateDto
{
}

public sealed class UpdateSrtTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

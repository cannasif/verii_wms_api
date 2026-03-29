using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.SubcontractingIssueTransfer.Dtos;

public sealed class SitLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreateSitLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class UpdateSitLineDto : BaseLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? OrderId { get; set; }
    [StringLength(10)]
    public string? ErpLineReference { get; set; }
}

public sealed class SitImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class SitImportLineWithRoutesDto
{
    public SitImportLineDto ImportLine { get; set; } = null!;
    public List<SitRouteDto> Routes { get; set; } = new();
}

public sealed class CreateSitImportLineDto : BaseImportLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class UpdateSitImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
    public long? LineId { get; set; }
    public long? RouteId { get; set; }
}

public sealed class AddSitImportBarcodeRequestDto
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

public sealed class SitRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreateSitRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateSitRouteDto
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

public sealed class SitLineSerialDto : BaseLineSerialEntityDto
{
    public long LineId { get; set; }
}

public sealed class CreateSitLineSerialDto : BaseLineSerialCreateDto
{
    [Required]
    public long LineId { get; set; }
}

public sealed class UpdateSitLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
}


public sealed class SitTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateSitTerminalLineDto : BaseTerminalLineCreateDto
{
}

public sealed class UpdateSitTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

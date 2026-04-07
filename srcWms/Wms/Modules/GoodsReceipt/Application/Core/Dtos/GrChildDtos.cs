using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.GoodsReceipt.Dtos;

public sealed class CreateGrLineDto : BaseLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
}

public sealed class UpdateGrLineDto : BaseLineUpdateDto
{
    [Required]
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
}

public sealed class CreateGrRouteDto : BaseRouteCreateDto
{
    [Required]
    public long ImportLineId { get; set; }
    public string? StockCode { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateGrRouteDto
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

public sealed class GrTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateGrTerminalLineDto : BaseTerminalLineCreateDto
{
    [Required]
    public long HeaderId { get; set; }
}

public sealed class UpdateGrTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}

public sealed class GrImportDocumentDto : BaseEntityDto
{
    [Required]
    public long HeaderId { get; set; }
    [Required]
    public byte[] Base64 { get; set; } = null!;
}

public sealed class GrImportLineWithRoutesDto
{
    public GrImportLineDto ImportLine { get; set; } = null!;
    public List<GrRouteDto> Routes { get; set; } = new();
}

public sealed class CreateGrImportLineDto : BaseImportLineCreateDto
{
    public long? LineId { get; set; }
    [Required]
    public long HeaderId { get; set; }
}

public sealed class UpdateGrImportLineDto : BaseImportLineUpdateDto
{
    public long? LineId { get; set; }
    public long? HeaderId { get; set; }
}

public sealed class AddGrImportBarcodeRequestDto
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

public sealed class UpdateGrLineSerialDto : BaseLineSerialUpdateDto
{
    public long? LineId { get; set; }
    public string? ClientKey { get; set; }
}

public sealed class CreateGrImportDocumentDto
{
    [Required]
    public long HeaderId { get; set; }
    [Required]
    public byte[] Base64 { get; set; } = null!;
}

public sealed class UpdateGrImportDocumentDto
{
    [Required]
    public long HeaderId { get; set; }
    [Required]
    public byte[] Base64 { get; set; } = null!;
}

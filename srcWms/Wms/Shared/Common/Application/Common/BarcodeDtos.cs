using System.ComponentModel.DataAnnotations;

namespace Wms.Application.Common;

public static class BarcodeModuleKeys
{
    public const string ProductLookup = "product-lookup";
    public const string GoodsReceiptAssigned = "goods-receipt-assigned";
    public const string WarehouseInboundAssigned = "warehouse-inbound-assigned";
    public const string WarehouseOutboundAssigned = "warehouse-outbound-assigned";
    public const string WarehouseTransferAssigned = "warehouse-transfer-assigned";
    public const string ShippingAssigned = "shipping-assigned";
    public const string SubcontractingIssueAssigned = "subcontracting-issue-assigned";
    public const string SubcontractingReceiptAssigned = "subcontracting-receipt-assigned";
}

public enum BarcodeMatchReasonCode
{
    None = 0,
    ParsedByDefinition = 1,
    ResolvedByMirrorLookup = 2,
    InvalidBarcodeFormat = 3,
    MissingRequiredSegment = 4,
    DefinitionNotFound = 5,
    NoMatch = 6,
    AmbiguousMatch = 7
}

public static class BarcodeDefinitionTypes
{
    public const string Pattern = "pattern";
    public const string Function = "function";
}

public sealed class BarcodeDefinitionDto
{
    public long? Id { get; set; }
    public string ModuleKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DefinitionType { get; set; } = BarcodeDefinitionTypes.Pattern;
    public string SegmentPattern { get; set; } = string.Empty;
    public string RequiredSegments { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool AllowMirrorLookup { get; set; }
    public string HintText { get; set; } = string.Empty;
    public string Source { get; set; } = "config";
    public bool IsEditable { get; set; }
    public string BranchCode { get; set; } = "0";
}

public sealed class SaveBarcodeDefinitionRequestDto
{
    [Required]
    [StringLength(100)]
    public string ModuleKey { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string DefinitionType { get; set; } = BarcodeDefinitionTypes.Pattern;

    [StringLength(250)]
    public string SegmentPattern { get; set; } = string.Empty;

    [StringLength(250)]
    public string RequiredSegments { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public bool AllowMirrorLookup { get; set; } = true;

    [StringLength(250)]
    public string HintText { get; set; } = string.Empty;
}

public sealed class BarcodeSegmentValueDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public sealed class BarcodeParseResultDto
{
    public bool Success { get; set; }
    public BarcodeMatchReasonCode ReasonCode { get; set; }
    public string RawBarcode { get; set; } = string.Empty;
    public string? SegmentPattern { get; set; }
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? SerialNumber { get; set; }
    public string? PackageNo { get; set; }
    public string? LotNo { get; set; }
    public string? BatchNo { get; set; }
    public string? Unit { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public decimal? Quantity { get; set; }
    public List<BarcodeSegmentValueDto> Segments { get; set; } = new();
    public string? Message { get; set; }
}

public sealed class ResolveBarcodeRequestDto
{
    [Required]
    [StringLength(100)]
    public string ModuleKey { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Barcode { get; set; } = string.Empty;

    [StringLength(50)]
    public string? FallbackStockCode { get; set; }

    [StringLength(255)]
    public string? FallbackStockName { get; set; }

    [StringLength(50)]
    public string? FallbackYapKod { get; set; }

    [StringLength(255)]
    public string? FallbackYapAcik { get; set; }

    [StringLength(100)]
    public string? FallbackSerialNumber { get; set; }
}

public sealed class BarcodeMatchCandidateDto
{
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? SerialNumber { get; set; }
}

public sealed class ResolvedBarcodeDto
{
    public string ModuleKey { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public string? SerialNumber { get; set; }
    public string? LotNo { get; set; }
    public string? BatchNo { get; set; }
    public string? PackageNo { get; set; }
    public string? Unit { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public decimal? Quantity { get; set; }
    public string? Source { get; set; }
    public string? DefinitionType { get; set; }
    public string? SegmentPattern { get; set; }
    public BarcodeMatchReasonCode ReasonCode { get; set; }
    public List<BarcodeSegmentValueDto> Segments { get; set; } = new();
    public List<BarcodeMatchCandidateDto> Candidates { get; set; } = new();
}

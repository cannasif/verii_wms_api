using Wms.Domain.Entities.Common;

namespace Wms.Application.Common;

public sealed class AssignedBarcodeRouteSnapshot
{
    public long? LineId { get; set; }
    public string? ScannedBarcode { get; set; }
    public string? SerialNo { get; set; }
    public decimal Quantity { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public sealed class AssignedBarcodeMatchRequest<TLine, TLineSerial>
    where TLine : BaseLineEntity
    where TLineSerial : BaseLineSerialEntity
{
    public required ResolveBarcodeRequestDto BarcodeRequest { get; init; }
    public required decimal RequestQuantity { get; init; }
    public required string RawBarcode { get; init; }
    public string? SourceCellCode { get; init; }
    public string? TargetCellCode { get; init; }
    public bool AllowMoreQuantityBasedOnOrder { get; init; }
    public required IReadOnlyCollection<TLine> Lines { get; init; }
    public required IReadOnlyCollection<TLineSerial> LineSerials { get; init; }
    public required IReadOnlyCollection<AssignedBarcodeRouteSnapshot> ExistingRoutes { get; init; }
    public required Func<TLine, long> LineIdSelector { get; init; }
    public required Func<TLineSerial, long?> LineSerialLineIdSelector { get; init; }
    public required string StockAndYapKodNotMatchedErrorCode { get; init; }
    public required string SerialNotMatchedErrorCode { get; init; }
    public required string NoMatchingLineErrorCode { get; init; }
    public required string QuantityExceededErrorCode { get; init; }
}

public sealed class AssignedBarcodeMatchResult
{
    public bool Success { get; private init; }
    public string? ErrorCode { get; private init; }
    public int? StatusCode { get; private init; }
    public object? Details { get; private init; }
    public ResolvedBarcodeDto? ResolvedBarcode { get; private init; }
    public string RequestedStockCode { get; private init; } = string.Empty;
    public string? RequestedYapKod { get; private init; }
    public string? RequestedSerialNo { get; private init; }
    public long? SelectedLineId { get; private init; }

    public static AssignedBarcodeMatchResult Error(string errorCode, int statusCode, object? details = null) =>
        new()
        {
            Success = false,
            ErrorCode = errorCode,
            StatusCode = statusCode,
            Details = details
        };

    public static AssignedBarcodeMatchResult Ok(
        ResolvedBarcodeDto resolvedBarcode,
        string requestedStockCode,
        string? requestedYapKod,
        string? requestedSerialNo,
        long selectedLineId) =>
        new()
        {
            Success = true,
            ResolvedBarcode = resolvedBarcode,
            RequestedStockCode = requestedStockCode,
            RequestedYapKod = requestedYapKod,
            RequestedSerialNo = requestedSerialNo,
            SelectedLineId = selectedLineId
        };
}

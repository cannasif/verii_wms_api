using Wms.Application.Common;
using Wms.Domain.Entities.Common;

namespace Wms.Infrastructure.Services.Common;

public sealed class AssignedBarcodeMatchingService : IAssignedBarcodeMatchingService
{
    private const decimal QuantityTolerance = 0.000001m;
    private readonly IBarcodeResolutionService _barcodeResolutionService;

    public AssignedBarcodeMatchingService(IBarcodeResolutionService barcodeResolutionService)
    {
        _barcodeResolutionService = barcodeResolutionService;
    }

    public async Task<AssignedBarcodeMatchResult> MatchAsync<TLine, TLineSerial>(
        AssignedBarcodeMatchRequest<TLine, TLineSerial> request,
        CancellationToken cancellationToken = default)
        where TLine : BaseLineEntity
        where TLineSerial : BaseLineSerialEntity
    {
        var resolvedBarcode = await _barcodeResolutionService.ResolveAsync(request.BarcodeRequest, cancellationToken);

        if (resolvedBarcode.ReasonCode is BarcodeMatchReasonCode.AmbiguousMatch
            or BarcodeMatchReasonCode.InvalidBarcodeFormat
            or BarcodeMatchReasonCode.MissingRequiredSegment
            or BarcodeMatchReasonCode.DefinitionNotFound
            or BarcodeMatchReasonCode.NoMatch)
        {
            return BuildResolutionError(resolvedBarcode);
        }

        var requestedStockCode = Normalize(resolvedBarcode.StockCode ?? request.BarcodeRequest.FallbackStockCode);
        var requestedYapKod = Normalize(resolvedBarcode.YapKod ?? request.BarcodeRequest.FallbackYapKod);
        var requestedSerialNo = Normalize(resolvedBarcode.SerialNumber ?? request.BarcodeRequest.FallbackSerialNumber);

        if (string.IsNullOrWhiteSpace(requestedStockCode))
        {
            return AssignedBarcodeMatchResult.Error("BarcodeCouldNotBeResolved", 400);
        }

        var matchingLines = request.Lines
            .Where(line =>
                string.Equals(Normalize(line.StockCode), requestedStockCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Normalize(line.YapKod), requestedYapKod, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingLines.Count == 0)
        {
            return AssignedBarcodeMatchResult.Error(request.StockAndYapKodNotMatchedErrorCode, 404);
        }

        var matchingLineIds = matchingLines
            .Select(request.LineIdSelector)
            .Distinct()
            .ToHashSet();

        var matchingLineSerials = request.LineSerials
            .Where(serial =>
            {
                var lineId = request.LineSerialLineIdSelector(serial);
                return lineId.HasValue && matchingLineIds.Contains(lineId.Value);
            })
            .ToList();

        var hasRequestSerial = !string.IsNullOrWhiteSpace(requestedSerialNo);
        var hasSerialBoundDemand = matchingLineSerials.Any(serial => !string.IsNullOrWhiteSpace(Normalize(serial.SerialNo)));

        if (hasSerialBoundDemand && hasRequestSerial)
        {
            var serialBoundLineSerials = matchingLineSerials
                .Where(serial => string.Equals(Normalize(serial.SerialNo), requestedSerialNo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (serialBoundLineSerials.Count == 0)
            {
                return AssignedBarcodeMatchResult.Error(request.SerialNotMatchedErrorCode, 404);
            }

            var totalLineSerialQuantity = serialBoundLineSerials.Sum(serial => serial.Quantity);
            var totalRouteQuantity = request.ExistingRoutes
                .Where(route =>
                    route.LineId.HasValue
                    && matchingLineIds.Contains(route.LineId.Value)
                    && string.Equals(Normalize(route.SerialNo), requestedSerialNo, StringComparison.OrdinalIgnoreCase))
                .Sum(route => route.Quantity);

            if (!request.AllowMoreQuantityBasedOnOrder
                && totalRouteQuantity + request.RequestQuantity > totalLineSerialQuantity + QuantityTolerance)
            {
                return AssignedBarcodeMatchResult.Error(request.QuantityExceededErrorCode, 400);
            }
        }
        else
        {
            var totalLineSerialQuantity = matchingLineSerials.Sum(serial => serial.Quantity);
            var totalRouteQuantity = request.ExistingRoutes
                .Where(route => route.LineId.HasValue && matchingLineIds.Contains(route.LineId.Value))
                .Sum(route => route.Quantity);

            if (!request.AllowMoreQuantityBasedOnOrder
                && totalRouteQuantity + request.RequestQuantity > totalLineSerialQuantity + QuantityTolerance)
            {
                return AssignedBarcodeMatchResult.Error(request.QuantityExceededErrorCode, 400);
            }
        }

        long? selectedLineId = null;

        if (hasSerialBoundDemand && hasRequestSerial)
        {
            var linesWithMatchingSerial = matchingLineSerials
                .Where(serial => string.Equals(Normalize(serial.SerialNo), requestedSerialNo, StringComparison.OrdinalIgnoreCase))
                .Select(request.LineSerialLineIdSelector)
                .Where(lineId => lineId.HasValue)
                .Select(lineId => lineId!.Value)
                .Distinct()
                .ToList();

            if (linesWithMatchingSerial.Count == 1)
            {
                selectedLineId = linesWithMatchingSerial[0];
            }
        }

        if (!selectedLineId.HasValue)
        {
            var lineQuantities = matchingLines
                .Select(line =>
                {
                    var lineId = request.LineIdSelector(line);
                    var lineSerialQuantity = matchingLineSerials
                        .Where(serial => request.LineSerialLineIdSelector(serial) == lineId)
                        .Sum(serial => serial.Quantity);
                    var routeQuantity = request.ExistingRoutes
                        .Where(route => route.LineId == lineId)
                        .Sum(route => route.Quantity);

                    return new
                    {
                        LineId = lineId,
                        Remaining = lineSerialQuantity - routeQuantity
                    };
                })
                .OrderByDescending(item => item.Remaining)
                .ToList();

            selectedLineId = lineQuantities.FirstOrDefault()?.LineId ?? matchingLineIds.FirstOrDefault();
        }

        if (!selectedLineId.HasValue || selectedLineId.Value <= 0)
        {
            return AssignedBarcodeMatchResult.Error(request.NoMatchingLineErrorCode, 400);
        }

        var duplicateRouteExists = request.ExistingRoutes.Any(route =>
            string.Equals(Normalize(route.ScannedBarcode), Normalize(request.RawBarcode), StringComparison.OrdinalIgnoreCase)
            && string.Equals(Normalize(route.SerialNo), requestedSerialNo, StringComparison.OrdinalIgnoreCase)
            && route.Quantity == request.RequestQuantity
            && string.Equals(Normalize(route.SourceCellCode), Normalize(request.SourceCellCode), StringComparison.OrdinalIgnoreCase)
            && string.Equals(Normalize(route.TargetCellCode), Normalize(request.TargetCellCode), StringComparison.OrdinalIgnoreCase));

        if (duplicateRouteExists)
        {
            return AssignedBarcodeMatchResult.Error("BarcodeAlreadyScanned", 409);
        }

        return AssignedBarcodeMatchResult.Ok(
            resolvedBarcode,
            requestedStockCode,
            string.IsNullOrWhiteSpace(requestedYapKod) ? null : requestedYapKod,
            string.IsNullOrWhiteSpace(requestedSerialNo) ? null : requestedSerialNo,
            selectedLineId.Value);
    }

    private static AssignedBarcodeMatchResult BuildResolutionError(ResolvedBarcodeDto resolvedBarcode)
    {
        return resolvedBarcode.ReasonCode switch
        {
            BarcodeMatchReasonCode.InvalidBarcodeFormat =>
                AssignedBarcodeMatchResult.Error("BarcodeInvalidFormat", 400, resolvedBarcode),
            BarcodeMatchReasonCode.MissingRequiredSegment =>
                AssignedBarcodeMatchResult.Error("BarcodeMissingRequiredSegment", 400, resolvedBarcode),
            BarcodeMatchReasonCode.DefinitionNotFound =>
                AssignedBarcodeMatchResult.Error("BarcodeDefinitionNotFound", 404, resolvedBarcode),
            BarcodeMatchReasonCode.NoMatch =>
                AssignedBarcodeMatchResult.Error("BarcodeNoMatch", 404, resolvedBarcode),
            BarcodeMatchReasonCode.AmbiguousMatch =>
                AssignedBarcodeMatchResult.Error("BarcodeAmbiguousMatch", 409, resolvedBarcode),
            _ =>
                AssignedBarcodeMatchResult.Error("BarcodeCouldNotBeResolved", 400, resolvedBarcode)
        };
    }

    private static string Normalize(string? value) => (value ?? string.Empty).Trim();
}

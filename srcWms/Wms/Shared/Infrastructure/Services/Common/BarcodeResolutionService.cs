using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using StockEntity = Wms.Domain.Entities.Stock.Stock;
using YapKodEntity = Wms.Domain.Entities.YapKod.YapKod;

namespace Wms.Infrastructure.Services.Common;

public sealed class BarcodeResolutionService : IBarcodeResolutionService
{
    private readonly IBarcodeDefinitionService _barcodeDefinitionService;
    private readonly IBarcodeParser _barcodeParser;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IRepository<YapKodEntity> _yapKodlar;

    public BarcodeResolutionService(
        IBarcodeDefinitionService barcodeDefinitionService,
        IBarcodeParser barcodeParser,
        IRepository<StockEntity> stocks,
        IRepository<YapKodEntity> yapKodlar)
    {
        _barcodeDefinitionService = barcodeDefinitionService;
        _barcodeParser = barcodeParser;
        _stocks = stocks;
        _yapKodlar = yapKodlar;
    }

    public async Task<ResolvedBarcodeDto> ResolveAsync(ResolveBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var trimmedModuleKey = (request.ModuleKey ?? string.Empty).Trim();
        var trimmedBarcode = (request.Barcode ?? string.Empty).Trim();
        var definition = await _barcodeDefinitionService.GetDefinitionAsync(trimmedModuleKey, cancellationToken);

        if (definition == null)
        {
            return BuildFallbackResult(request, BarcodeMatchReasonCode.DefinitionNotFound);
        }

        BarcodeParseResultDto? parsed = null;
        if (definition.IsActive
            && string.Equals(definition.DefinitionType, BarcodeDefinitionTypes.Pattern, StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(definition.SegmentPattern))
        {
            parsed = _barcodeParser.Parse(trimmedBarcode, definition.SegmentPattern);
            if (parsed.Success)
            {
                var missing = ValidateRequiredSegments(parsed, definition.RequiredSegments);
                if (missing.Count > 0)
                {
                    return BuildMissingRequiredSegmentResult(request, definition, parsed, missing);
                }

                var resolved = new ResolvedBarcodeDto
                {
                    ModuleKey = trimmedModuleKey,
                    Barcode = trimmedBarcode,
                    StockCode = Coalesce(parsed.StockCode, request.FallbackStockCode),
                    YapKod = Coalesce(parsed.YapKod, request.FallbackYapKod),
                    YapAcik = Coalesce(parsed.YapAcik, request.FallbackYapAcik),
                    SerialNumber = Coalesce(parsed.SerialNumber, request.FallbackSerialNumber),
                    LotNo = parsed.LotNo,
                    BatchNo = parsed.BatchNo,
                    PackageNo = parsed.PackageNo,
                    Unit = parsed.Unit,
                    SourceWarehouseCode = parsed.SourceWarehouseCode,
                    TargetWarehouseCode = parsed.TargetWarehouseCode,
                    SourceCellCode = parsed.SourceCellCode,
                    TargetCellCode = parsed.TargetCellCode,
                    Quantity = parsed.Quantity,
                    Source = "pattern",
                    DefinitionType = definition.DefinitionType,
                    SegmentPattern = definition.SegmentPattern,
                    ReasonCode = BarcodeMatchReasonCode.ParsedByDefinition,
                    Segments = parsed.Segments
                };

                await EnrichFromMirrorsAsync(resolved, cancellationToken);
                resolved.StockName ??= request.FallbackStockName;
                return resolved;
            }
        }

        if (definition.AllowMirrorLookup)
        {
            var mirrorResult = await ResolveFromMirrorsAsync(trimmedModuleKey, trimmedBarcode, definition, request, cancellationToken);
            if (mirrorResult != null)
            {
                return mirrorResult;
            }
        }

        return BuildFallbackResult(request, BarcodeMatchReasonCode.NoMatch, definition);
    }

    private static List<string> ValidateRequiredSegments(BarcodeParseResultDto parsed, string? requiredSegments)
    {
        if (string.IsNullOrWhiteSpace(requiredSegments))
        {
            return new List<string>();
        }

        var missing = new List<string>();
        foreach (var segment in requiredSegments.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var value = segment.ToUpperInvariant() switch
            {
                "STOCKCODE" => parsed.StockCode,
                "YAPKOD" => parsed.YapKod,
                "SERIALNUMBER" => parsed.SerialNumber,
                "PACKAGENO" => parsed.PackageNo,
                "LOTNO" => parsed.LotNo,
                "BATCHNO" => parsed.BatchNo,
                "QUANTITY" => parsed.Quantity?.ToString(),
                "UNIT" => parsed.Unit,
                "SOURCEWAREHOUSECODE" => parsed.SourceWarehouseCode,
                "TARGETWAREHOUSECODE" => parsed.TargetWarehouseCode,
                "SOURCECELLCODE" => parsed.SourceCellCode,
                "TARGETCELLCODE" => parsed.TargetCellCode,
                _ => null
            };

            if (string.IsNullOrWhiteSpace(value))
            {
                missing.Add(segment);
            }
        }

        return missing;
    }

    private static ResolvedBarcodeDto BuildMissingRequiredSegmentResult(
        ResolveBarcodeRequestDto request,
        BarcodeDefinitionDto definition,
        BarcodeParseResultDto parsed,
        IReadOnlyCollection<string> missingSegments)
    {
        return new ResolvedBarcodeDto
        {
            ModuleKey = (request.ModuleKey ?? string.Empty).Trim(),
            Barcode = (request.Barcode ?? string.Empty).Trim(),
            StockCode = Coalesce(parsed.StockCode, request.FallbackStockCode),
            StockName = Coalesce(request.FallbackStockName),
            YapKod = Coalesce(parsed.YapKod, request.FallbackYapKod),
            YapAcik = Coalesce(request.FallbackYapAcik),
            SerialNumber = Coalesce(parsed.SerialNumber, request.FallbackSerialNumber),
            LotNo = parsed.LotNo,
            BatchNo = parsed.BatchNo,
            PackageNo = parsed.PackageNo,
            Unit = parsed.Unit,
            SourceWarehouseCode = parsed.SourceWarehouseCode,
            TargetWarehouseCode = parsed.TargetWarehouseCode,
            SourceCellCode = parsed.SourceCellCode,
            TargetCellCode = parsed.TargetCellCode,
            Quantity = parsed.Quantity,
            Source = "pattern",
            DefinitionType = definition.DefinitionType,
            SegmentPattern = definition.SegmentPattern,
            ReasonCode = BarcodeMatchReasonCode.MissingRequiredSegment,
            Segments = parsed.Segments.Concat(missingSegments.Select(x => new BarcodeSegmentValueDto { Name = "MissingRequiredSegment", Value = x })).ToList()
        };
    }

    private async Task EnrichFromMirrorsAsync(ResolvedBarcodeDto resolved, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(resolved.StockCode))
        {
            var stock = await _stocks.Query()
                .Where(x => !x.IsDeleted && x.ErpStockCode == resolved.StockCode)
                .Select(x => new { x.StockName })
                .FirstOrDefaultAsync(cancellationToken);
            if (stock != null)
            {
                resolved.StockName = stock.StockName;
            }
        }

        if (!string.IsNullOrWhiteSpace(resolved.YapKod))
        {
            var yapKod = await _yapKodlar.Query()
                .Where(x => !x.IsDeleted && x.YapKodCode == resolved.YapKod)
                .Select(x => new { x.YapAcik })
                .FirstOrDefaultAsync(cancellationToken);
            if (yapKod != null)
            {
                resolved.YapAcik = yapKod.YapAcik;
            }
        }
    }

    private async Task<ResolvedBarcodeDto?> ResolveFromMirrorsAsync(
        string moduleKey,
        string barcode,
        BarcodeDefinitionDto definition,
        ResolveBarcodeRequestDto request,
        CancellationToken cancellationToken)
    {
        var normalizedBarcode = NormalizeCode(barcode);
        var normalizedFallbackStockCode = NormalizeCode(request.FallbackStockCode);
        var normalizedFallbackYapKod = NormalizeCode(request.FallbackYapKod);

        var matchedStocks = await _stocks.Query()
            .Where(x => !x.IsDeleted)
            .Where(x =>
                (!string.IsNullOrWhiteSpace(normalizedBarcode) && (
                    x.ErpStockCode == normalizedBarcode
                    || x.UreticiKodu == normalizedBarcode
                    || x.Kod1 == normalizedBarcode
                    || x.Kod2 == normalizedBarcode
                    || x.Kod3 == normalizedBarcode
                    || x.Kod4 == normalizedBarcode
                    || x.Kod5 == normalizedBarcode))
                || (!string.IsNullOrWhiteSpace(normalizedFallbackStockCode) && x.ErpStockCode == normalizedFallbackStockCode))
            .Select(x => new
            {
                x.ErpStockCode,
                x.StockName
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        if (matchedStocks.Count > 1)
        {
            return new ResolvedBarcodeDto
            {
                ModuleKey = moduleKey,
                Barcode = barcode,
                Source = "mirror",
                DefinitionType = definition.DefinitionType,
                SegmentPattern = definition.SegmentPattern,
                ReasonCode = BarcodeMatchReasonCode.AmbiguousMatch,
                Candidates = matchedStocks
                    .Take(5)
                    .Select(x => new BarcodeMatchCandidateDto
                    {
                        StockCode = x.ErpStockCode,
                        StockName = x.StockName,
                        YapKod = request.FallbackYapKod,
                        YapAcik = request.FallbackYapAcik,
                        SerialNumber = request.FallbackSerialNumber
                    })
                    .ToList()
            };
        }

        if (matchedStocks.Count == 0)
        {
            return null;
        }

        var matchedStock = matchedStocks[0];
        string? yapKod = request.FallbackYapKod;
        string? yapAcik = request.FallbackYapAcik;

        if (!string.IsNullOrWhiteSpace(normalizedFallbackYapKod))
        {
            var mirroredYapKod = await _yapKodlar.Query()
                .Where(x => !x.IsDeleted && x.YapKodCode == normalizedFallbackYapKod)
                .Select(x => new { x.YapKodCode, x.YapAcik })
                .FirstOrDefaultAsync(cancellationToken);

            if (mirroredYapKod == null)
            {
                return BuildFallbackResult(request, BarcodeMatchReasonCode.NoMatch, definition);
            }

            yapKod = mirroredYapKod.YapKodCode;
            yapAcik = mirroredYapKod.YapAcik;
        }

        return new ResolvedBarcodeDto
        {
            ModuleKey = moduleKey,
            Barcode = barcode,
            StockCode = matchedStock.ErpStockCode,
            StockName = matchedStock.StockName,
            YapKod = Coalesce(yapKod),
            YapAcik = Coalesce(yapAcik),
            SerialNumber = Coalesce(request.FallbackSerialNumber),
            Source = "mirror",
            DefinitionType = definition.DefinitionType,
            SegmentPattern = definition.SegmentPattern,
            ReasonCode = BarcodeMatchReasonCode.ResolvedByMirrorLookup,
            Segments = new List<BarcodeSegmentValueDto>
            {
                new() { Name = "Barcode", Value = barcode },
                new() { Name = "StockCode", Value = matchedStock.ErpStockCode ?? string.Empty },
                new() { Name = "YapKod", Value = yapKod ?? string.Empty }
            }
        };
    }

    private static ResolvedBarcodeDto BuildFallbackResult(ResolveBarcodeRequestDto request, BarcodeMatchReasonCode reasonCode, BarcodeDefinitionDto? definition = null)
    {
        return new ResolvedBarcodeDto
        {
            ModuleKey = (request.ModuleKey ?? string.Empty).Trim(),
            Barcode = (request.Barcode ?? string.Empty).Trim(),
            StockCode = Coalesce(request.FallbackStockCode),
            StockName = Coalesce(request.FallbackStockName),
            YapKod = Coalesce(request.FallbackYapKod),
            YapAcik = Coalesce(request.FallbackYapAcik),
            SerialNumber = Coalesce(request.FallbackSerialNumber),
            Source = "fallback",
            DefinitionType = definition?.DefinitionType,
            SegmentPattern = definition?.SegmentPattern,
            ReasonCode = reasonCode,
            Segments = new List<BarcodeSegmentValueDto>()
        };
    }

    private static string? Coalesce(params string?[] values)
    {
        return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim();
    }

    private static string? NormalizeCode(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    }
}

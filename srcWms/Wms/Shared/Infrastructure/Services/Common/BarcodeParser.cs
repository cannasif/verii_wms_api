using System.Globalization;
using System.Text.RegularExpressions;
using Wms.Application.Common;

namespace Wms.Infrastructure.Services.Common;

public sealed class BarcodeParser : IBarcodeParser
{
    private static readonly Regex TokenRegex = new("[A-Za-z][A-Za-z0-9]*", RegexOptions.Compiled);

    public BarcodeParseResultDto Parse(string rawBarcode, string format)
    {
        rawBarcode = (rawBarcode ?? string.Empty).Trim();
        format = (format ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(rawBarcode) || string.IsNullOrWhiteSpace(format))
        {
            return new BarcodeParseResultDto
            {
                Success = false,
                RawBarcode = rawBarcode,
                SegmentPattern = format,
                ReasonCode = BarcodeMatchReasonCode.InvalidBarcodeFormat,
                Message = "Barcode format is not configured."
            };
        }

        var tokens = TokenRegex.Matches(format).Cast<Match>().ToList();
        if (tokens.Count == 0)
        {
            return new BarcodeParseResultDto
            {
                Success = false,
                RawBarcode = rawBarcode,
                SegmentPattern = format,
                ReasonCode = BarcodeMatchReasonCode.InvalidBarcodeFormat,
                Message = "Barcode format contains no segments."
            };
        }

        if (tokens[0].Index != 0 || tokens[^1].Index + tokens[^1].Length != format.Length)
        {
            return new BarcodeParseResultDto
            {
                Success = false,
                RawBarcode = rawBarcode,
                SegmentPattern = format,
                ReasonCode = BarcodeMatchReasonCode.InvalidBarcodeFormat,
                Message = "Barcode format must start and end with a field name."
            };
        }

        var segments = new List<(string Name, string Delimiter)>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var delimiterStart = tokens[i].Index + tokens[i].Length;
            var delimiterEnd = i + 1 < tokens.Count ? tokens[i + 1].Index : format.Length;
            var delimiter = format.Substring(delimiterStart, delimiterEnd - delimiterStart);
            segments.Add((tokens[i].Value, delimiter));
        }

        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var cursor = 0;
        for (var i = 0; i < segments.Count; i++)
        {
            var (name, delimiter) = segments[i];
            if (string.IsNullOrEmpty(delimiter))
            {
                values[name] = rawBarcode[cursor..].Trim();
                cursor = rawBarcode.Length;
                continue;
            }

            var delimiterIndex = rawBarcode.IndexOf(delimiter, cursor, StringComparison.Ordinal);
            if (delimiterIndex < 0)
            {
                return new BarcodeParseResultDto
                {
                    Success = false,
                    RawBarcode = rawBarcode,
                    SegmentPattern = format,
                    ReasonCode = BarcodeMatchReasonCode.InvalidBarcodeFormat,
                    Message = $"Delimiter '{delimiter}' was not found in the barcode value."
                };
            }

            values[name] = rawBarcode.Substring(cursor, delimiterIndex - cursor).Trim();
            cursor = delimiterIndex + delimiter.Length;
        }

        var result = new BarcodeParseResultDto
        {
            Success = values.Count > 0,
            RawBarcode = rawBarcode,
            SegmentPattern = format,
            ReasonCode = values.Count > 0 ? BarcodeMatchReasonCode.ParsedByDefinition : BarcodeMatchReasonCode.InvalidBarcodeFormat,
            Segments = values.Select(x => new BarcodeSegmentValueDto { Name = x.Key, Value = x.Value }).ToList(),
            StockCode = GetValue(values, "StockCode", "StokKodu", "Stock", "ItemCode"),
            YapKod = GetValue(values, "YapKod", "ConfigurationCode", "ConfigCode", "VariantCode"),
            SerialNumber = GetValue(values, "SerialNumber", "SerialNo", "SeriNo"),
            PackageNo = GetValue(values, "PackageNo", "PackageBarcode", "PackageCode"),
            LotNo = GetValue(values, "LotNo", "Lot"),
            BatchNo = GetValue(values, "BatchNo", "Batch", "BatchCode"),
            Unit = GetValue(values, "Unit", "UnitCode", "OlcuBr"),
            SourceWarehouseCode = GetValue(values, "SourceWarehouseCode", "SourceWarehouse", "WarehouseCode", "DepoKodu"),
            TargetWarehouseCode = GetValue(values, "TargetWarehouseCode", "TargetWarehouse", "HedefDepoKodu"),
            SourceCellCode = GetValue(values, "SourceCellCode", "SourceCell", "RafKodu", "HucreKodu"),
            TargetCellCode = GetValue(values, "TargetCellCode", "TargetCell", "HedefRafKodu")
        };

        var quantityValue = GetValue(values, "Quantity", "Qty", "Miktar");
        if (!string.IsNullOrWhiteSpace(quantityValue)
            && decimal.TryParse(quantityValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedQuantity))
        {
            result.Quantity = parsedQuantity;
        }

        return result;
    }

    private static string? GetValue(IReadOnlyDictionary<string, string> values, params string[] aliases)
    {
        foreach (var alias in aliases)
        {
            if (values.TryGetValue(alias, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }
}

using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public static class WiProcessGroupingHelper
{
    public static IReadOnlyList<WiProcessImportLineSeed> BuildImportLineSeeds(IEnumerable<ProcessWiRouteDto>? routes)
    {
        if (routes == null)
        {
            return Array.Empty<WiProcessImportLineSeed>();
        }

        var grouped = new Dictionary<string, WiProcessImportLineSeed>(StringComparer.OrdinalIgnoreCase);
        foreach (var route in routes)
        {
            var groupingKey = BuildGroupingKey(route.StockId, route.StockCode, route.YapKodId, route.YapKod);
            if (!grouped.ContainsKey(groupingKey))
            {
                grouped[groupingKey] = new WiProcessImportLineSeed(
                    groupingKey,
                    route.StockId,
                    route.StockCode,
                    route.YapKodId,
                    route.YapKod);
            }
        }

        return grouped.Values.ToArray();
    }

    public static string BuildGroupingKey(long? stockId, string stockCode, long? yapKodId, string? yapKod)
    {
        var stockKey = stockId.HasValue
            ? $"STOCK-ID:{stockId.Value}"
            : $"STOCK-CODE:{(stockCode ?? string.Empty).Trim().ToUpperInvariant()}";
        var yapKey = yapKodId.HasValue
            ? $"YAP-ID:{yapKodId.Value}"
            : $"YAP-CODE:{(yapKod ?? string.Empty).Trim().ToUpperInvariant()}";

        return $"HEADER|{stockKey}|{yapKey}";
    }
}

public sealed record WiProcessImportLineSeed(
    string GroupingKey,
    long? StockId,
    string StockCode,
    long? YapKodId,
    string? YapKod);

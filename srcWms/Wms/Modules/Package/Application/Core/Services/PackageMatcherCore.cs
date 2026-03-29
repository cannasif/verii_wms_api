using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

internal static class PackageMatcherCore
{
    public static async Task<ApiResponse<long>> MatchAsync<THeader, TLine, TLineSerial, TImportLine, TRoute, TParameter>(
        PHeader header,
        PLine packageLine,
        IRepository<THeader> headers,
        IRepository<TLine> lines,
        IRepository<TLineSerial> lineSerials,
        IRepository<TImportLine> importLines,
        IRepository<TRoute> routes,
        IRepository<TParameter> parameters,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        Func<long, long, string, string?, TImportLine> importFactory,
        Func<long, PLine, long?, long?, TRoute> routeFactory,
        CancellationToken cancellationToken)
        where THeader : BaseHeaderEntity
        where TLine : BaseLineEntity
        where TLineSerial : BaseLineSerialEntity
        where TImportLine : BaseImportLineEntity
        where TRoute : BaseRouteEntity
        where TParameter : BaseEntity
    {
        if (!header.SourceHeaderId.HasValue)
        {
            return Error<long>(localizationService, "SourceHeaderIdNotFound", 400);
        }

        var sourceHeader = await headers.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
        if (sourceHeader == null || sourceHeader.IsDeleted || sourceHeader.IsCompleted)
        {
            return Error<long>(localizationService, "MatchedSourceHeaderMustBeActiveAndIncomplete", 400);
        }

        var packageStockCode = (packageLine.StockCode ?? string.Empty).Trim();
        var packageYapKod = (packageLine.YapKod ?? string.Empty).Trim();
        var matchingLines = await lines.Query()
            .Where(line => EF.Property<long>(line, "HeaderId") == header.SourceHeaderId.Value
                && !line.IsDeleted
                && (line.StockCode ?? string.Empty).Trim() == packageStockCode
                && (line.YapKod ?? string.Empty).Trim() == packageYapKod)
            .ToListAsync(cancellationToken);
        if (matchingLines.Count == 0)
        {
            return Error<long>(localizationService, "PLineStockCodeAndYapKodNotMatch", 404, $"PLine Id {packageLine.Id}: StockCode ({packageStockCode}) and YapKod ({packageYapKod}) do not match any Line in SourceHeader");
        }

        var lineIds = matchingLines.Select(x => x.Id).ToList();
        var serials = await lineSerials.Query().Where(ls => !ls.IsDeleted && lineIds.Contains(EF.Property<long>(ls, "LineId"))).ToListAsync(cancellationToken);
        dynamic? parameter = await parameters.Query().FirstOrDefaultAsync(cancellationToken);

        var serialNo = (packageLine.SerialNo ?? string.Empty).Trim();
        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);
        var hasSerialInLineSerials = serials.Any(ls => !string.IsNullOrWhiteSpace(ls.SerialNo));

        if (hasSerialInLineSerials && hasRequestSerial)
        {
            var matchingLineSerials = serials.Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo).ToList();
            if (matchingLineSerials.Count == 0)
            {
                return Error<long>(localizationService, "PLineSerialNotMatch", 404, $"PLine Id {packageLine.Id}: SerialNo ({serialNo}) does not match any LineSerial");
            }

            var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
            var totalRouteQuantity = await routes.Query()
                .Where(r => !r.IsDeleted && EF.Property<long>(r, "ImportLineId") > 0)
                .Join(importLines.Query().Where(il => !il.IsDeleted && EF.Property<long?>(il, "LineId").HasValue),
                    r => EF.Property<long>(r, "ImportLineId"),
                    il => il.Id,
                    (r, il) => new { Route = r, ImportLine = il })
                .Where(x => lineIds.Contains(EF.Property<long?>(x.ImportLine, "LineId")!.Value) && (x.Route.SerialNo ?? string.Empty).Trim() == serialNo)
                .SumAsync(x => x.Route.Quantity, cancellationToken);

            if (!(parameter?.AllowMoreQuantityBasedOnOrder ?? false) && totalRouteQuantity + packageLine.Quantity > totalLineSerialQuantity + 0.000001m)
            {
                var localizedMessage = localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                var exceptionMessage = $"Serial {serialNo} (StockCode: {packageStockCode}, YapKod: {packageYapKod}): Route total after add ({totalRouteQuantity + packageLine.Quantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                return ApiResponse<long>.ErrorResult(localizedMessage, exceptionMessage, 400);
            }
        }
        else
        {
            var totalLineSerialQuantity = serials.Sum(ls => ls.Quantity);
            var totalRouteQuantity = await routes.Query()
                .Where(r => !r.IsDeleted && EF.Property<long>(r, "ImportLineId") > 0)
                .Join(importLines.Query().Where(il => !il.IsDeleted && EF.Property<long?>(il, "LineId").HasValue),
                    r => EF.Property<long>(r, "ImportLineId"),
                    il => il.Id,
                    (r, il) => new { Route = r, ImportLine = il })
                .Where(x => lineIds.Contains(EF.Property<long?>(x.ImportLine, "LineId")!.Value))
                .SumAsync(x => x.Route.Quantity, cancellationToken);

            if (!(parameter?.AllowMoreQuantityBasedOnOrder ?? false) && totalRouteQuantity + packageLine.Quantity > totalLineSerialQuantity + 0.000001m)
            {
                var localizedMessage = localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                var exceptionMessage = $"StockCode: {packageStockCode}, YapKod: {packageYapKod}: Route total after add ({totalRouteQuantity + packageLine.Quantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                return ApiResponse<long>.ErrorResult(localizedMessage, exceptionMessage, 400);
            }
        }

        long? selectedLineId = null;
        if (hasSerialInLineSerials && hasRequestSerial)
        {
            var linesWithSerial = serials.Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo).Select(ls => EF.Property<long>(ls, "LineId")).Distinct().ToList();
            if (linesWithSerial.Count == 1)
            {
                selectedLineId = linesWithSerial.First();
            }
        }

        if (!selectedLineId.HasValue)
        {
            var lineQuantities = new List<(long LineId, decimal Remaining)>();
            foreach (var line in matchingLines)
            {
                var lineId = line.Id;
                var lineSerialTotal = serials.Where(ls => EF.Property<long>(ls, "LineId") == lineId).Sum(ls => ls.Quantity);
                var routeTotal = await routes.Query()
                    .Where(r => !r.IsDeleted && EF.Property<long>(r, "ImportLineId") > 0)
                    .Join(importLines.Query().Where(il => !il.IsDeleted && EF.Property<long?>(il, "LineId").HasValue),
                        r => EF.Property<long>(r, "ImportLineId"),
                        il => il.Id,
                        (r, il) => new { Route = r, ImportLine = il })
                    .Where(x => EF.Property<long?>(x.ImportLine, "LineId") == lineId)
                    .SumAsync(x => x.Route.Quantity, cancellationToken);
                lineQuantities.Add((lineId, lineSerialTotal - routeTotal));
            }
            var bestLine = lineQuantities.OrderByDescending(x => x.Remaining).FirstOrDefault();
            selectedLineId = bestLine.LineId > 0 ? bestLine.LineId : matchingLines.First().Id;
        }

        var existingImportLine = await importLines.Query(tracking: true)
            .Where(il => EF.Property<long>(il, "HeaderId") == header.SourceHeaderId.Value
                && EF.Property<long?>(il, "LineId") == selectedLineId.Value
                && (il.StockCode ?? string.Empty).Trim() == packageStockCode
                && (il.YapKod ?? string.Empty).Trim() == packageYapKod
                && !il.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingImportLine == null)
        {
            existingImportLine = importFactory(header.SourceHeaderId.Value, selectedLineId.Value, packageStockCode, packageYapKod);
            await importLines.AddAsync(existingImportLine, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        int? sourceWarehouse = int.TryParse(GetStringValue(sourceHeader, "SourceWarehouse"), out var sw) ? sw : null;
        int? targetWarehouse = int.TryParse(GetStringValue(sourceHeader, "TargetWarehouse"), out var tw) ? tw : null;
        var route = routeFactory(existingImportLine.Id, packageLine, sourceWarehouse, targetWarehouse);
        await routes.AddAsync(route, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<long>.SuccessResult(route.Id, localizationService.GetLocalizedString("PHeaderMatchedSuccessfully"));
    }

    private static string? GetStringValue(object source, string propertyName)
        => source.GetType().GetProperty(propertyName)?.GetValue(source) as string;

    private static ApiResponse<T> Error<T>(ILocalizationService localizationService, string key, int statusCode, string? exceptionMessage = null)
    {
        var message = localizationService.GetLocalizedString(key);
        return ApiResponse<T>.ErrorResult(message, exceptionMessage ?? message, statusCode);
    }
}

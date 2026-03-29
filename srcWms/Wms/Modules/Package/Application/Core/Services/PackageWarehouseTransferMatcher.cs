using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.Package.Services;

public sealed class PackageWarehouseTransferMatcher : IPackageWarehouseTransferMatcher
{
    private readonly IRepository<WtHeader> _headers;
    private readonly IRepository<WtLine> _lines;
    private readonly IRepository<WtLineSerial> _lineSerials;
    private readonly IRepository<WtImportLine> _importLines;
    private readonly IRepository<WtRoute> _routes;
    private readonly IRepository<WtParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public PackageWarehouseTransferMatcher(
        IRepository<WtHeader> headers,
        IRepository<WtLine> lines,
        IRepository<WtLineSerial> lineSerials,
        IRepository<WtImportLine> importLines,
        IRepository<WtRoute> routes,
        IRepository<WtParameter> parameters,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService)
    {
        _headers = headers;
        _lines = lines;
        _lineSerials = lineSerials;
        _importLines = importLines;
        _routes = routes;
        _parameters = parameters;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<long>> MatchPackageLineToWarehouseTransferAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default)
    {
        if (!header.SourceHeaderId.HasValue)
        {
            return Error<long>("SourceHeaderIdNotFound", 400);
        }

        var sourceHeader = await _headers.Query(tracking: true)
            .Where(x => x.Id == header.SourceHeaderId.Value)
            .FirstOrDefaultAsync(cancellationToken);
        if (sourceHeader == null || sourceHeader.IsDeleted || sourceHeader.IsCompleted)
        {
            return Error<long>("MatchedSourceHeaderMustBeActiveAndIncomplete", 400);
        }

        var packageStockCode = (packageLine.StockCode ?? string.Empty).Trim();
        var packageYapKod = (packageLine.YapKod ?? string.Empty).Trim();
        var matchingLines = await _lines.Query()
            .Where(line => line.HeaderId == header.SourceHeaderId.Value
                && !line.IsDeleted
                && (line.StockCode ?? string.Empty).Trim() == packageStockCode
                && (line.YapKod ?? string.Empty).Trim() == packageYapKod)
            .ToListAsync(cancellationToken);
        if (matchingLines.Count == 0)
        {
            return Error<long>("PLineStockCodeAndYapKodNotMatch", 404, $"PLine Id {packageLine.Id}: StockCode ({packageStockCode}) and YapKod ({packageYapKod}) do not match any Line in SourceHeader");
        }

        var lineIds = matchingLines.Select(x => x.Id).ToList();
        var lineSerials = await _lineSerials.Query()
            .Where(ls => !ls.IsDeleted && lineIds.Contains(ls.LineId))
            .ToListAsync(cancellationToken);
        var parameter = await _parameters.Query().FirstOrDefaultAsync(cancellationToken);

        var serialNo = (packageLine.SerialNo ?? string.Empty).Trim();
        var hasRequestSerial = !string.IsNullOrWhiteSpace(serialNo);
        var hasSerialInLineSerials = lineSerials.Any(ls => !string.IsNullOrWhiteSpace(ls.SerialNo));

        if (hasSerialInLineSerials && hasRequestSerial)
        {
            var matchingLineSerials = lineSerials.Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo).ToList();
            if (matchingLineSerials.Count == 0)
            {
                return Error<long>("PLineSerialNotMatch", 404, $"PLine Id {packageLine.Id}: SerialNo ({serialNo}) does not match any LineSerial");
            }

            var totalLineSerialQuantity = matchingLineSerials.Sum(ls => ls.Quantity);
            var totalRouteQuantity = await _routes.Query()
                .Where(r => !r.IsDeleted && r.ImportLine != null && !r.ImportLine.IsDeleted && r.ImportLine.LineId.HasValue && lineIds.Contains(r.ImportLine.LineId.Value) && (r.SerialNo ?? string.Empty).Trim() == serialNo)
                .SumAsync(r => r.Quantity, cancellationToken);
            if (!(parameter?.AllowMoreQuantityBasedOnOrder ?? false) && totalRouteQuantity + packageLine.Quantity > totalLineSerialQuantity + 0.000001m)
            {
                var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                var exceptionMessage = $"Serial {serialNo} (StockCode: {packageStockCode}, YapKod: {packageYapKod}): Route total after add ({totalRouteQuantity + packageLine.Quantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                return ApiResponse<long>.ErrorResult(localizedMessage, exceptionMessage, 400);
            }
        }
        else
        {
            var totalLineSerialQuantity = lineSerials.Sum(ls => ls.Quantity);
            var totalRouteQuantity = await _routes.Query()
                .Where(r => !r.IsDeleted && r.ImportLine != null && !r.ImportLine.IsDeleted && r.ImportLine.LineId.HasValue && lineIds.Contains(r.ImportLine.LineId.Value))
                .SumAsync(r => r.Quantity, cancellationToken);
            if (!(parameter?.AllowMoreQuantityBasedOnOrder ?? false) && totalRouteQuantity + packageLine.Quantity > totalLineSerialQuantity + 0.000001m)
            {
                var localizedMessage = _localizationService.GetLocalizedString("PLineQuantityCannotBeGreater");
                var exceptionMessage = $"StockCode: {packageStockCode}, YapKod: {packageYapKod}: Route total after add ({totalRouteQuantity + packageLine.Quantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
                return ApiResponse<long>.ErrorResult(localizedMessage, exceptionMessage, 400);
            }
        }

        long? selectedLineId = null;
        if (hasSerialInLineSerials && hasRequestSerial)
        {
            var linesWithSerial = lineSerials
                .Where(ls => (ls.SerialNo ?? string.Empty).Trim() == serialNo)
                .Select(ls => ls.LineId)
                .Distinct()
                .ToList();
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
                var lineSerialTotal = lineSerials.Where(ls => ls.LineId == line.Id).Sum(ls => ls.Quantity);
                var routeTotal = await _routes.Query()
                    .Where(r => !r.IsDeleted && r.ImportLine != null && !r.ImportLine.IsDeleted && r.ImportLine.LineId == line.Id)
                    .SumAsync(r => r.Quantity, cancellationToken);
                lineQuantities.Add((line.Id, lineSerialTotal - routeTotal));
            }

            var bestLine = lineQuantities.OrderByDescending(x => x.Remaining).FirstOrDefault();
            selectedLineId = bestLine.LineId > 0 ? bestLine.LineId : matchingLines.First().Id;
        }

        var existingImportLine = await _importLines.Query(tracking: true)
            .Where(il => il.HeaderId == header.SourceHeaderId.Value
                && il.LineId == selectedLineId.Value
                && (il.StockCode ?? string.Empty).Trim() == packageStockCode
                && (il.YapKod ?? string.Empty).Trim() == packageYapKod
                && !il.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingImportLine == null)
        {
            existingImportLine = new WtImportLine
            {
                HeaderId = header.SourceHeaderId.Value,
                LineId = selectedLineId.Value,
                StockCode = packageStockCode,
                YapKod = packageYapKod,
                CreatedDate = DateTimeProvider.Now
            };
            await _importLines.AddAsync(existingImportLine, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var route = new WtRoute
        {
            ImportLineId = existingImportLine.Id,
            ScannedBarcode = packageLine.Barcode ?? string.Empty,
            Quantity = packageLine.Quantity,
            SerialNo = packageLine.SerialNo,
            SerialNo2 = packageLine.SerialNo2,
            SerialNo3 = packageLine.SerialNo3,
            SerialNo4 = packageLine.SerialNo4,
            SourceWarehouse = int.TryParse(sourceHeader.SourceWarehouse, out var sourceWarehouse) ? sourceWarehouse : null,
            TargetWarehouse = int.TryParse(sourceHeader.TargetWarehouse, out var targetWarehouse) ? targetWarehouse : null,
            CreatedDate = DateTimeProvider.Now
        };
        await _routes.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<long>.SuccessResult(route.Id, _localizationService.GetLocalizedString("PHeaderMatchedSuccessfully"));
    }

    private ApiResponse<T> Error<T>(string key, int statusCode, string? exceptionMessage = null)
    {
        var message = _localizationService.GetLocalizedString(key);
        return ApiResponse<T>.ErrorResult(message, exceptionMessage ?? message, statusCode);
    }
}

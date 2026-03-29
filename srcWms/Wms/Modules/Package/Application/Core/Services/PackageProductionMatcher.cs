using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Package.Services;

public sealed class PackageProductionMatcher : IPackageProductionMatcher
{
    private readonly IRepository<PrHeader> _headers;
    private readonly IRepository<PrLine> _lines;
    private readonly IRepository<PrLineSerial> _lineSerials;
    private readonly IRepository<PrImportLine> _importLines;
    private readonly IRepository<PrRoute> _routes;
    private readonly IRepository<PrParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public PackageProductionMatcher(IRepository<PrHeader> headers, IRepository<PrLine> lines, IRepository<PrLineSerial> lineSerials, IRepository<PrImportLine> importLines, IRepository<PrRoute> routes, IRepository<PrParameter> parameters, IUnitOfWork unitOfWork, ILocalizationService localizationService)
    {
        _headers = headers; _lines = lines; _lineSerials = lineSerials; _importLines = importLines; _routes = routes; _parameters = parameters; _unitOfWork = unitOfWork; _localizationService = localizationService;
    }

    public Task<ApiResponse<long>> MatchPackageLineToProductionAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default)
        => PackageMatcherCore.MatchAsync(header, packageLine, _headers, _lines, _lineSerials, _importLines, _routes, _parameters, _unitOfWork, _localizationService, (headerId, lineId, stockCode, yapKod) => new PrImportLine { HeaderId = headerId, LineId = lineId, StockCode = stockCode, YapKod = yapKod, CreatedDate = DateTimeProvider.Now }, (importLineId, packageLine, sourceWarehouse, targetWarehouse) => new PrRoute { ImportLineId = importLineId, ScannedBarcode = packageLine.Barcode ?? string.Empty, Quantity = packageLine.Quantity, SerialNo = packageLine.SerialNo, SerialNo2 = packageLine.SerialNo2, SerialNo3 = packageLine.SerialNo3, SerialNo4 = packageLine.SerialNo4, SourceWarehouse = sourceWarehouse.HasValue ? (int?)sourceWarehouse.Value : null, TargetWarehouse = targetWarehouse.HasValue ? (int?)targetWarehouse.Value : null, CreatedDate = DateTimeProvider.Now }, cancellationToken);
}

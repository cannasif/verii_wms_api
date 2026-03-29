using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.ProductionTransfer;

namespace Wms.Application.Package.Services;

public sealed class PackageProductionTransferMatcher : IPackageProductionTransferMatcher
{
    private readonly IRepository<PtHeader> _headers;
    private readonly IRepository<PtLine> _lines;
    private readonly IRepository<PtLineSerial> _lineSerials;
    private readonly IRepository<PtImportLine> _importLines;
    private readonly IRepository<PtRoute> _routes;
    private readonly IRepository<PtParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public PackageProductionTransferMatcher(IRepository<PtHeader> headers, IRepository<PtLine> lines, IRepository<PtLineSerial> lineSerials, IRepository<PtImportLine> importLines, IRepository<PtRoute> routes, IRepository<PtParameter> parameters, IUnitOfWork unitOfWork, ILocalizationService localizationService)
    {
        _headers = headers; _lines = lines; _lineSerials = lineSerials; _importLines = importLines; _routes = routes; _parameters = parameters; _unitOfWork = unitOfWork; _localizationService = localizationService;
    }

    public Task<ApiResponse<long>> MatchPackageLineToProductionTransferAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default)
        => PackageMatcherCore.MatchAsync(header, packageLine, _headers, _lines, _lineSerials, _importLines, _routes, _parameters, _unitOfWork, _localizationService, (headerId, lineId, stockCode, yapKod) => new PtImportLine { HeaderId = headerId, LineId = lineId, StockCode = stockCode, YapKod = yapKod, CreatedDate = DateTimeProvider.Now }, (importLineId, packageLine, sourceWarehouse, targetWarehouse) => new PtRoute { ImportLineId = importLineId, ScannedBarcode = packageLine.Barcode ?? string.Empty, Quantity = packageLine.Quantity, SerialNo = packageLine.SerialNo, SerialNo2 = packageLine.SerialNo2, SerialNo3 = packageLine.SerialNo3, SerialNo4 = packageLine.SerialNo4, SourceWarehouse = sourceWarehouse.HasValue ? (int?)sourceWarehouse.Value : null, TargetWarehouse = targetWarehouse.HasValue ? (int?)targetWarehouse.Value : null, CreatedDate = DateTimeProvider.Now }, cancellationToken);
}

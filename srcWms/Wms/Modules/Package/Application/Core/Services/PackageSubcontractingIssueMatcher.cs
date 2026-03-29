using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.SubcontractingIssueTransfer;

namespace Wms.Application.Package.Services;

public sealed class PackageSubcontractingIssueMatcher : IPackageSubcontractingIssueMatcher
{
    private readonly IRepository<SitHeader> _headers;
    private readonly IRepository<SitLine> _lines;
    private readonly IRepository<SitLineSerial> _lineSerials;
    private readonly IRepository<SitImportLine> _importLines;
    private readonly IRepository<SitRoute> _routes;
    private readonly IRepository<SitParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public PackageSubcontractingIssueMatcher(IRepository<SitHeader> headers, IRepository<SitLine> lines, IRepository<SitLineSerial> lineSerials, IRepository<SitImportLine> importLines, IRepository<SitRoute> routes, IRepository<SitParameter> parameters, IUnitOfWork unitOfWork, ILocalizationService localizationService)
    {
        _headers = headers; _lines = lines; _lineSerials = lineSerials; _importLines = importLines; _routes = routes; _parameters = parameters; _unitOfWork = unitOfWork; _localizationService = localizationService;
    }

    public Task<ApiResponse<long>> MatchPackageLineToSubcontractingIssueAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default)
        => PackageMatcherCore.MatchAsync(header, packageLine, _headers, _lines, _lineSerials, _importLines, _routes, _parameters, _unitOfWork, _localizationService, (headerId, lineId, stockCode, yapKod) => new SitImportLine { HeaderId = headerId, LineId = lineId, StockCode = stockCode, YapKod = yapKod, CreatedDate = DateTimeProvider.Now }, (importLineId, packageLine, sourceWarehouse, targetWarehouse) => new SitRoute { ImportLineId = importLineId, ScannedBarcode = packageLine.Barcode ?? string.Empty, Quantity = packageLine.Quantity, SerialNo = packageLine.SerialNo, SerialNo2 = packageLine.SerialNo2, SerialNo3 = packageLine.SerialNo3, SerialNo4 = packageLine.SerialNo4, SourceWarehouse = sourceWarehouse.HasValue ? (int?)sourceWarehouse.Value : null, TargetWarehouse = targetWarehouse.HasValue ? (int?)targetWarehouse.Value : null, CreatedDate = DateTimeProvider.Now }, cancellationToken);
}

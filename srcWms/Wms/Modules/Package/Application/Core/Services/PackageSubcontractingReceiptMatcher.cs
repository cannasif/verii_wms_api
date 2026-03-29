using Wms.Application.Common;
using Wms.Domain.Common;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;

namespace Wms.Application.Package.Services;

public sealed class PackageSubcontractingReceiptMatcher : IPackageSubcontractingReceiptMatcher
{
    private readonly IRepository<SrtHeader> _headers;
    private readonly IRepository<SrtLine> _lines;
    private readonly IRepository<SrtLineSerial> _lineSerials;
    private readonly IRepository<SrtImportLine> _importLines;
    private readonly IRepository<SrtRoute> _routes;
    private readonly IRepository<SrtParameter> _parameters;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public PackageSubcontractingReceiptMatcher(IRepository<SrtHeader> headers, IRepository<SrtLine> lines, IRepository<SrtLineSerial> lineSerials, IRepository<SrtImportLine> importLines, IRepository<SrtRoute> routes, IRepository<SrtParameter> parameters, IUnitOfWork unitOfWork, ILocalizationService localizationService)
    {
        _headers = headers; _lines = lines; _lineSerials = lineSerials; _importLines = importLines; _routes = routes; _parameters = parameters; _unitOfWork = unitOfWork; _localizationService = localizationService;
    }

    public Task<ApiResponse<long>> MatchPackageLineToSubcontractingReceiptAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default)
        => PackageMatcherCore.MatchAsync(header, packageLine, _headers, _lines, _lineSerials, _importLines, _routes, _parameters, _unitOfWork, _localizationService, (headerId, lineId, stockCode, yapKod) => new SrtImportLine { HeaderId = headerId, LineId = lineId, StockCode = stockCode, YapKod = yapKod, CreatedDate = DateTimeProvider.Now }, (importLineId, packageLine, sourceWarehouse, targetWarehouse) => new SrtRoute { ImportLineId = importLineId, ScannedBarcode = packageLine.Barcode ?? string.Empty, Quantity = packageLine.Quantity, SerialNo = packageLine.SerialNo, SerialNo2 = packageLine.SerialNo2, SerialNo3 = packageLine.SerialNo3, SerialNo4 = packageLine.SerialNo4, SourceWarehouse = sourceWarehouse.HasValue ? (int?)sourceWarehouse.Value : null, TargetWarehouse = targetWarehouse.HasValue ? (int?)targetWarehouse.Value : null, CreatedDate = DateTimeProvider.Now }, cancellationToken);
}

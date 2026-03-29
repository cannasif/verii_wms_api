using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageWarehouseInboundMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToWarehouseInboundAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

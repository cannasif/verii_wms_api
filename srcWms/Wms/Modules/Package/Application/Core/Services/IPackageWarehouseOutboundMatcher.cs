using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageWarehouseOutboundMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToWarehouseOutboundAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

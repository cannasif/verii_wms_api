using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageProductionMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToProductionAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

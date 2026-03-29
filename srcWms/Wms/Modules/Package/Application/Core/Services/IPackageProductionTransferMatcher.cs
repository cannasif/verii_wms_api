using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageProductionTransferMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToProductionTransferAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

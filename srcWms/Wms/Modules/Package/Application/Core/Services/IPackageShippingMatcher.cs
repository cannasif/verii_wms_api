using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageShippingMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToShippingAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

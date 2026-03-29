using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageWarehouseTransferMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToWarehouseTransferAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

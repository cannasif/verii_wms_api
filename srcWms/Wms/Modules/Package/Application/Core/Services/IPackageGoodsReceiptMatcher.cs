using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageGoodsReceiptMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToGoodsReceiptAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

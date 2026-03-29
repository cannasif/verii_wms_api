using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageSubcontractingReceiptMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToSubcontractingReceiptAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

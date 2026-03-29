using Wms.Application.Common;
using Wms.Domain.Entities.Package;

namespace Wms.Application.Package.Services;

public interface IPackageSubcontractingIssueMatcher
{
    Task<ApiResponse<long>> MatchPackageLineToSubcontractingIssueAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken = default);
}

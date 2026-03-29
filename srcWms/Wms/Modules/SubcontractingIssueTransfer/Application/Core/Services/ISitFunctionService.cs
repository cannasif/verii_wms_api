using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitFunctionService
{
    Task<ApiResponse<List<SitOpenOrderHeaderDto>>> GetSitOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<SitOpenOrderLineDto>>> GetSitOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

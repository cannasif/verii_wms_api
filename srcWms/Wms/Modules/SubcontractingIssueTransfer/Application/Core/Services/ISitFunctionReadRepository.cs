using Wms.Domain.Entities.SubcontractingIssueTransfer.Functions;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitFunctionReadRepository
{
    Task<List<FnSitOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnSitOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

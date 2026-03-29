using Wms.Domain.Entities.SubcontractingReceiptTransfer.Functions;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public interface ISrtFunctionReadRepository
{
    Task<List<FnSrtOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnSrtOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

using Wms.Domain.Entities.Shipping.Functions;

namespace Wms.Application.Shipping.Services;

public interface IShFunctionReadRepository
{
    Task<List<FnTransferOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnTransferOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

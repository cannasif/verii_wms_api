using Wms.Domain.Entities.WarehouseTransfer.Functions;

namespace Wms.Application.WarehouseTransfer.Services;

public interface IWtFunctionReadRepository
{
    Task<List<FnTransferOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnTransferOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

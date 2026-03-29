using Wms.Domain.Entities.WarehouseOutbound.Functions;

namespace Wms.Application.WarehouseOutbound.Services;

public interface IWoFunctionReadRepository
{
    Task<List<FnWoOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnWoOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

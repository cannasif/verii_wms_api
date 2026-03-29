using Wms.Domain.Entities.WarehouseInbound.Functions;

namespace Wms.Application.WarehouseInbound.Services;

public interface IWiFunctionReadRepository
{
    Task<List<FnWiOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnWiOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default);
}

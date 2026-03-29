using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public interface IWiFunctionService
{
    Task<ApiResponse<List<WiOpenOrderHeaderDto>>> GetWiOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<WiOpenOrderLineDto>>> GetWiOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

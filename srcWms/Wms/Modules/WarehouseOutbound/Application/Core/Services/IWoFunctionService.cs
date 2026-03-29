using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;

namespace Wms.Application.WarehouseOutbound.Services;

public interface IWoFunctionService
{
    Task<ApiResponse<List<WoOpenOrderHeaderDto>>> GetWoOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<WoOpenOrderLineDto>>> GetWoOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

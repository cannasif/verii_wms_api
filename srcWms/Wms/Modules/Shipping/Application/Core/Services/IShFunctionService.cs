using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public interface IShFunctionService
{
    Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetShippingOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetShippingOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

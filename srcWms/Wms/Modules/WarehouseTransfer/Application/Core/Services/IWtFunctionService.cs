using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;

namespace Wms.Application.WarehouseTransfer.Services;

public interface IWtFunctionService
{
    Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetTransferOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetTransferOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

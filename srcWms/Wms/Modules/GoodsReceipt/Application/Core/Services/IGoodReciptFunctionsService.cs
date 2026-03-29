using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;

namespace Wms.Application.GoodsReceipt.Services;

public interface IGoodReciptFunctionsService
{
    Task<ApiResponse<List<GoodsOpenOrdersHeaderDto>>> GetGoodsReceiptHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineAsync(string siparisNoCsv, string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(string branchCode, string customerCode, CancellationToken cancellationToken = default);
}

using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGoodReciptFunctionsService
    {
        
        Task<ApiResponse<List<GoodsOpenOrdersHeaderDto>>> GetGoodsReceiptHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineAsync(string siparisNoCsv, string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<GoodsOpenOrdersLineDto>>> GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(string branchCode, string customerCode, CancellationToken cancellationToken = default);

    }
}

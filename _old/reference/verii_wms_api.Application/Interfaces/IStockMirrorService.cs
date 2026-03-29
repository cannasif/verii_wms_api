using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IStockMirrorService
    {
        Task<ApiResponse<PagedResponse<StockPagedDto>>> GetPagedAsync(PagedRequest request);
    }
}

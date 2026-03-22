using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IStockMirrorService
    {
        Task<ApiResponse<PagedResponse<StockPagedDto>>> GetPagedAsync(PagedRequest request);
    }
}

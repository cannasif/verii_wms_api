using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiFunctionService
    {
        Task<ApiResponse<List<WiOpenOrderHeaderDto>>> GetWiOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<WiOpenOrderLineDto>>> GetWiOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
    }
}

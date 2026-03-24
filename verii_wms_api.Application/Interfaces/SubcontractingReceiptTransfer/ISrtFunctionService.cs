using System.Collections.Generic;
using System.Threading.Tasks;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtFunctionService
    {
        Task<ApiResponse<List<SrtOpenOrderHeaderDto>>> GetSrtOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<SrtOpenOrderLineDto>>> GetSrtOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
    }
}

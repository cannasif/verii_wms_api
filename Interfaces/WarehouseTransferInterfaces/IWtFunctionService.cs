using WMS_WEBAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtFunctionService
    {
        Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetTransferOpenOrderHeaderAsync(string customerCode);
        Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetTransferOpenOrderLineAsync(string siparisNoCsv);
        
    }
}
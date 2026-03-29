using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public interface ISrtFunctionService
{
    Task<ApiResponse<List<SrtOpenOrderHeaderDto>>> GetSrtOpenOrderHeaderAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<SrtOpenOrderLineDto>>> GetSrtOpenOrderLineAsync(string siparisNoCsv, CancellationToken cancellationToken = default);
}

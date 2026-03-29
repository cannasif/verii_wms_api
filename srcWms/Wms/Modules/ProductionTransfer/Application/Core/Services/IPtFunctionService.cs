using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtFunctionService
{
    Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);
}

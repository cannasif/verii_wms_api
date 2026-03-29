using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrFunctionService
{
    Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);
}

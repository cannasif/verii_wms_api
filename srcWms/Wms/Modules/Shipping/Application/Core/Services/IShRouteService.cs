using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public interface IShRouteService
{
    Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

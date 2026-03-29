using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;

namespace Wms.Application.WarehouseTransfer.Services;

public interface IWtRouteService
{
    Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

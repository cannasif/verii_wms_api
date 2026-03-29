using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcRouteService
{
    Task<ApiResponse<IEnumerable<IcRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<IcRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<IcRouteDto>>> GetByImportLineIdAsync(long importLineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcRouteDto>> CreateAsync(CreateIcRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcRouteDto>> UpdateAsync(long id, UpdateIcRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

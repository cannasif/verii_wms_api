using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtRouteService
{
    Task<ApiResponse<IEnumerable<PtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtRouteDto>> CreateAsync(CreatePtRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtRouteDto>> UpdateAsync(long id, UpdatePtRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

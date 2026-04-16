using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;

namespace Wms.Application.ServiceAllocation.Services;

public interface IOrderAllocationLineService
{
    Task<ApiResponse<IEnumerable<OrderAllocationLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<OrderAllocationLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderAllocationLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderAllocationLineDto>> CreateAsync(CreateOrderAllocationLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderAllocationLineDto>> UpdateAsync(long id, UpdateOrderAllocationLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<AllocationRecomputeResultDto>> RecomputeAsync(RecomputeAllocationRequestDto request, CancellationToken cancellationToken = default);
}

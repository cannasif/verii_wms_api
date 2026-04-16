using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;

namespace Wms.Application.ServiceAllocation.Services;

public interface IAllocationEngine
{
    Task<ApiResponse<AllocationRecomputeResultDto>> RecomputeAsync(RecomputeAllocationRequestDto request, CancellationToken cancellationToken = default);
}

using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;

namespace Wms.Application.ServiceAllocation.Services;

public interface IServiceCaseService
{
    Task<ApiResponse<IEnumerable<ServiceCaseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ServiceCaseDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseTimelineDto>> GetTimelineAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseDto>> CreateAsync(CreateServiceCaseDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseDto>> UpdateAsync(long id, UpdateServiceCaseDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

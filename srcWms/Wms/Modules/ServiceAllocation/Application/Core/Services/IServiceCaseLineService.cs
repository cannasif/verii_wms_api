using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;

namespace Wms.Application.ServiceAllocation.Services;

public interface IServiceCaseLineService
{
    Task<ApiResponse<IEnumerable<ServiceCaseLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ServiceCaseLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseLineDto>> CreateAsync(CreateServiceCaseLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ServiceCaseLineDto>> UpdateAsync(long id, UpdateServiceCaseLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

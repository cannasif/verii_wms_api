using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitRouteService
{
    Task<ApiResponse<IEnumerable<SitRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitRouteDto>> CreateAsync(CreateSitRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitRouteDto>> UpdateAsync(long id, UpdateSitRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

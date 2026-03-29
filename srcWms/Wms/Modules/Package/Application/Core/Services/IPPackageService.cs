using Wms.Application.Common;
using Wms.Application.Package.Dtos;

namespace Wms.Application.Package.Services;

public interface IPPackageService
{
    Task<ApiResponse<IEnumerable<PPackageDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PPackageDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PPackageDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PPackageDto>>> GetByPackingHeaderIdAsync(long packingHeaderId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PPackageDto>> CreateAsync(CreatePPackageDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PPackageDto>> UpdateAsync(long id, UpdatePPackageDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

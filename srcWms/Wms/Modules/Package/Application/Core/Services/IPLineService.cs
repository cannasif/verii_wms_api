using Wms.Application.Common;
using Wms.Application.Package.Dtos;

namespace Wms.Application.Package.Services;

public interface IPLineService
{
    Task<ApiResponse<IEnumerable<PLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackageIdAsync(long packageId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackingHeaderIdAsync(long packingHeaderId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PLineDto>> CreateAsync(CreatePLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PLineDto>> UpdateAsync(long id, UpdatePLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

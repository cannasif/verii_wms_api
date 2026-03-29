using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcHeaderService
{
    Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcLineService
{
    Task<ApiResponse<IEnumerable<IcLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcLineDto>> CreateAsync(CreateIcLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcLineDto>> UpdateAsync(long id, UpdateIcLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

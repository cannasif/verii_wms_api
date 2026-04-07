using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcScopeService
{
    Task<ApiResponse<IEnumerable<IcScopeDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcScopeDto>> CreateAsync(CreateIcScopeDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcScopeDto>> UpdateAsync(long id, UpdateIcScopeDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

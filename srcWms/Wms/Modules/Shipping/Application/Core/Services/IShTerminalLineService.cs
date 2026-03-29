using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public interface IShTerminalLineService
{
    Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShTerminalLineDto>> CreateAsync(CreateShTerminalLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShTerminalLineDto>> UpdateAsync(long id, UpdateShTerminalLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

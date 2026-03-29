using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public interface IWiTerminalLineService
{
    Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiTerminalLineDto>> CreateAsync(CreateWiTerminalLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiTerminalLineDto>> UpdateAsync(long id, UpdateWiTerminalLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtTerminalLineService
{
    Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtTerminalLineDto>> CreateAsync(CreatePtTerminalLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtTerminalLineDto>> UpdateAsync(long id, UpdatePtTerminalLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

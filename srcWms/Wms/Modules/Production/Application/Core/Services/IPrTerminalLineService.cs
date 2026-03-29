using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrTerminalLineService
{
    Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrTerminalLineDto>> CreateAsync(CreatePrTerminalLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrTerminalLineDto>> UpdateAsync(long id, UpdatePrTerminalLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

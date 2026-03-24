using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoTerminalLineService
    {
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WoTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WoTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoTerminalLineDto>> CreateAsync(CreateWoTerminalLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoTerminalLineDto>> UpdateAsync(long id, UpdateWoTerminalLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

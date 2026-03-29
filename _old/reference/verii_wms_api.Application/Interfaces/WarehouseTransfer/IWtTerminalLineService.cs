using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtTerminalLineService
    {
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtTerminalLineDto>> CreateAsync(CreateWtTerminalLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WtTerminalLineDto>> UpdateAsync(long id, UpdateWtTerminalLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

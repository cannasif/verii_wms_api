using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtTerminalLineService
    {
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtTerminalLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WtTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WtTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<WtTerminalLineDto>> CreateAsync(CreateWtTerminalLineDto createDto);
        Task<ApiResponse<WtTerminalLineDto>> UpdateAsync(long id, UpdateWtTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

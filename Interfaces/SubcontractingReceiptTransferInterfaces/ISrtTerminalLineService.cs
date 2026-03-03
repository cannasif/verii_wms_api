using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtTerminalLineService
    {
        Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtTerminalLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SrtTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByUserIdAsync(long userId);
        Task<ApiResponse<SrtTerminalLineDto>> CreateAsync(CreateSrtTerminalLineDto createDto);
        Task<ApiResponse<SrtTerminalLineDto>> UpdateAsync(long id, UpdateSrtTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

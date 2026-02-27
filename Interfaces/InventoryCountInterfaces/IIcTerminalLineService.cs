using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcTerminalLineService
    {
        Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetAllAsync();
        Task<ApiResponse<IcTerminalLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IcTerminalLineDto>> CreateAsync(CreateIcTerminalLineDto createDto);
        Task<ApiResponse<IcTerminalLineDto>> UpdateAsync(long id, UpdateIcTerminalLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
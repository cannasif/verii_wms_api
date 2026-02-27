using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcHeaderService
    {
        Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync();
        Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto);
        Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
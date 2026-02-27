using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IMobilemenuHeaderService
    {
        Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetAllAsync();
        Task<ApiResponse<MobilemenuHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<MobilemenuHeaderDto>> GetByMenuIdAsync(string menuId);
        Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetByTitleAsync(string title);
        Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetOpenMenusAsync();
        Task<ApiResponse<MobilemenuHeaderDto>> CreateAsync(CreateMobilemenuHeaderDto createDto);
        Task<ApiResponse<MobilemenuHeaderDto>> UpdateAsync(long id, UpdateMobilemenuHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
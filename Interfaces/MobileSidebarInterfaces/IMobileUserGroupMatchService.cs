using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IMobileUserGroupMatchService
    {
        Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetAllAsync();
        Task<ApiResponse<MobileUserGroupMatchDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByUserIdAsync(int userId);
        Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByGroupCodeAsync(string groupCode);
        Task<ApiResponse<MobileUserGroupMatchDto>> CreateAsync(CreateMobileUserGroupMatchDto createDto);
        Task<ApiResponse<MobileUserGroupMatchDto>> UpdateAsync(long id, UpdateMobileUserGroupMatchDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPlatformUserGroupMatchService
    {
        Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PlatformUserGroupMatchDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PlatformUserGroupMatchDto>> GetByIdAsync(long id);
        Task<ApiResponse<PlatformUserGroupMatchDto>> CreateAsync(CreatePlatformUserGroupMatchDto createDto);
        Task<ApiResponse<PlatformUserGroupMatchDto>> UpdateAsync(long id, UpdatePlatformUserGroupMatchDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetByUserIdAsync(int userId);
        Task<ApiResponse<IEnumerable<PlatformUserGroupMatchDto>>> GetByGroupCodeAsync(string groupCode);
    }
}

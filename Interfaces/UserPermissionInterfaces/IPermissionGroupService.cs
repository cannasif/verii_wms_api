using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPermissionGroupService
    {
        Task<ApiResponse<PagedResponse<PermissionGroupDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<PermissionGroupDto>> GetByIdAsync(long id);
        Task<ApiResponse<PermissionGroupDto>> CreateAsync(CreatePermissionGroupDto dto);
        Task<ApiResponse<PermissionGroupDto>> UpdateAsync(long id, UpdatePermissionGroupDto dto);
        Task<ApiResponse<PermissionGroupDto>> SetPermissionsAsync(long id, SetPermissionGroupPermissionsDto dto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

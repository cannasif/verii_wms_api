using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IUserPermissionGroupService
    {
        Task<ApiResponse<UserPermissionGroupDto>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserPermissionGroupDto>> SetUserGroupsAsync(long userId, SetUserPermissionGroupsDto dto, CancellationToken cancellationToken = default);
    }
}

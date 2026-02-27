using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<PagedResponse<UserDto>>> GetAllUsersAsync(PagedRequest request);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(long id);
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<ApiResponse<UserDto>> UpdateUserAsync(long id, UpdateUserDto dto);
        Task<ApiResponse<object>> DeleteUserAsync(long id);
    }
}

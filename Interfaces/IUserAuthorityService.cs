using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IUserAuthorityService
    {
        Task<ApiResponse<IEnumerable<UserAuthorityDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<UserAuthorityDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<UserAuthorityDto>> GetByIdAsync(long id);
        Task<ApiResponse<UserAuthorityDto>> CreateAsync(CreateUserAuthorityDto createDto);
        Task<ApiResponse<UserAuthorityDto>> UpdateAsync(long id, UpdateUserAuthorityDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

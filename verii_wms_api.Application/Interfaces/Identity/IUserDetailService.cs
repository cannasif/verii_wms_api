using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IUserDetailService
    {
        Task<ApiResponse<UserDetailDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserDetailDto>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<UserDetailDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<UserDetailDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserDetailDto>> CreateAsync(CreateUserDetailDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserDetailDto>> UpdateAsync(long id, UpdateUserDetailDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

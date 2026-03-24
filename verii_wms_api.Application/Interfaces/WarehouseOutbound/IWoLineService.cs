using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoLineService
    {
        Task<ApiResponse<IEnumerable<WoLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WoLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WoLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoLineDto>> CreateAsync(CreateWoLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoLineDto>> UpdateAsync(long id, UpdateWoLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

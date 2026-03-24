using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoRouteService
    {
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WoRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoRouteDto>> CreateAsync(CreateWoRouteDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WoRouteDto>> UpdateAsync(long id, UpdateWoRouteDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

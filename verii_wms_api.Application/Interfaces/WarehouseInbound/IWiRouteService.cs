using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiRouteService
    {
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<WiRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiRouteDto>> CreateAsync(CreateWiRouteDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<WiRouteDto>> UpdateAsync(long id, UpdateWiRouteDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

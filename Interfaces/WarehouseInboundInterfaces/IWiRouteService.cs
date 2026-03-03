using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiRouteService
    {
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WiRouteDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WiRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<WiRouteDto>> CreateAsync(CreateWiRouteDto createDto);
        Task<ApiResponse<WiRouteDto>> UpdateAsync(long id, UpdateWiRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

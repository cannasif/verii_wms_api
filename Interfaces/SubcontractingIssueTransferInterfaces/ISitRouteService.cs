using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitRouteService
    {
        Task<ApiResponse<IEnumerable<SitRouteDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SitRouteDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SitRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<SitRouteDto>> CreateAsync(CreateSitRouteDto createDto);
        Task<ApiResponse<SitRouteDto>> UpdateAsync(long id, UpdateSitRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

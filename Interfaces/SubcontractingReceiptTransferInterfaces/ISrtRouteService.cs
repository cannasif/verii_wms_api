using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtRouteService
    {
        Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtRouteDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SrtRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<SrtRouteDto>> CreateAsync(CreateSrtRouteDto createDto);
        Task<ApiResponse<SrtRouteDto>> UpdateAsync(long id, UpdateSrtRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

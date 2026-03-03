using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtRouteService
    {
        Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtRouteDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto);
        Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

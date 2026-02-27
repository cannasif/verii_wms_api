using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcRouteService
    {
        Task<ApiResponse<IEnumerable<IcRouteDto>>> GetAllAsync();
        Task<ApiResponse<IcRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<IcRouteDto>>> GetByImportLineIdAsync(long importLineId);
        Task<ApiResponse<IcRouteDto>> CreateAsync(CreateIcRouteDto createDto);
        Task<ApiResponse<IcRouteDto>> UpdateAsync(long id, UpdateIcRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        
    }
}
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IIcHeaderService
    {
        Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto);
        Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
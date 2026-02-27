using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitLineSerialService
    {
        Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto);
        Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
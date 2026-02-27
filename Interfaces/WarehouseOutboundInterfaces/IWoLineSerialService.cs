using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoLineSerialService
    {
        Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<WoLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<WoLineSerialDto>> CreateAsync(CreateWoLineSerialDto createDto);
        Task<ApiResponse<WoLineSerialDto>> UpdateAsync(long id, UpdateWoLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
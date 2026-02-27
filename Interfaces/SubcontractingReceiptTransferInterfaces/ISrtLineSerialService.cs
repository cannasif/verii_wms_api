using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtLineSerialService
    {
        Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<SrtLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<SrtLineSerialDto>> CreateAsync(CreateSrtLineSerialDto createDto);
        Task<ApiResponse<SrtLineSerialDto>> UpdateAsync(long id, UpdateSrtLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
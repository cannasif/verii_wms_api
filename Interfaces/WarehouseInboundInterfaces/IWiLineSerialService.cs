using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiLineSerialService
    {
        Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto);
        Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
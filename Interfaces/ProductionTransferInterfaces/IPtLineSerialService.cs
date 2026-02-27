using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtLineSerialService
    {
        Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<PtLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<PtLineSerialDto>> CreateAsync(CreatePtLineSerialDto createDto);
        Task<ApiResponse<PtLineSerialDto>> UpdateAsync(long id, UpdatePtLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
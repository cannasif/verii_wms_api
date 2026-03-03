using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtLineSerialService
    {
        Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtLineSerialDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<WtLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<WtLineSerialDto>> CreateAsync(CreateWtLineSerialDto createDto);
        Task<ApiResponse<WtLineSerialDto>> UpdateAsync(long id, UpdateWtLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtLineSerialService
    {
        Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SrtLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtLineSerialDto>> CreateAsync(CreateSrtLineSerialDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SrtLineSerialDto>> UpdateAsync(long id, UpdateSrtLineSerialDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

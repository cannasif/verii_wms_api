using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitLineSerialService
    {
        Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SitLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

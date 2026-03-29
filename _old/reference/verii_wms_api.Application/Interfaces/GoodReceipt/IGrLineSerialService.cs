using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrLineSerialService
    {
        Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<GrLineSerialDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc", CancellationToken cancellationToken = default);
        Task<ApiResponse<GrLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<GrLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrLineSerialDto>> CreateAsync(CreateGrLineSerialDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrLineSerialDto>> UpdateAsync(long id, UpdateGrLineSerialDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrRouteService
    {
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<GrRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByImportLineIdAsync(long importLineId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrRouteDto>> CreateAsync(CreateGrRouteDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrRouteDto>> UpdateAsync(long id, UpdateGrRouteDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}

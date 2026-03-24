using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitParameterService
    {
        Task<ApiResponse<IEnumerable<SitParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<SitParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitParameterDto>> CreateAsync(CreateSitParameterDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<SitParameterDto>> UpdateAsync(long id, UpdateSitParameterDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}


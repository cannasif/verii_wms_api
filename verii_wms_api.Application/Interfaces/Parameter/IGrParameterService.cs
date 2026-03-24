using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrParameterService
    {
        Task<ApiResponse<IEnumerable<GrParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<GrParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrParameterDto>> CreateAsync(CreateGrParameterDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrParameterDto>> UpdateAsync(long id, UpdateGrParameterDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}


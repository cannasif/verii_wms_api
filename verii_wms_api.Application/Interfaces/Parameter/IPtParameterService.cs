using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtParameterService
    {
        Task<ApiResponse<IEnumerable<PtParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<PtParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<PtParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PtParameterDto>> CreateAsync(CreatePtParameterDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<PtParameterDto>> UpdateAsync(long id, UpdatePtParameterDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}


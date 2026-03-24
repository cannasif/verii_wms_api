using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShParameterService
    {
        Task<ApiResponse<IEnumerable<ShParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<ShParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<ShParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<ShParameterDto>> CreateAsync(CreateShParameterDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<ShParameterDto>> UpdateAsync(long id, UpdateShParameterDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}


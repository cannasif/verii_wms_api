using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrTerminalLineService
    {
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<PagedResponse<GrTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<GrTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrTerminalLineDto>> CreateAsync(CreateGrTerminalLineDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<GrTerminalLineDto>> UpdateAsync(long id, UpdateGrTerminalLineDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}


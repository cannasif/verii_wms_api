using Wms.Application.Common;
using Wms.Application.Package.Dtos;

namespace Wms.Application.Package.Services;

public interface IPHeaderService
{
    Task<ApiResponse<IEnumerable<PHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PHeaderDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PHeaderDto>> CreateAsync(CreatePHeaderDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PHeaderDto>> UpdateAsync(long id, UpdatePHeaderDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> MatchPlinesWithMatchedStatus(long pHeaderId, bool isMatched, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<object>>> GetAvailableHeadersForMappingAsync(string sourceType, CancellationToken cancellationToken = default);
}

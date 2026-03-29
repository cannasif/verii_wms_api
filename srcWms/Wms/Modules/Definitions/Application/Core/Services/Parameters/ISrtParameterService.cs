using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

public interface ISrtParameterService
{
    Task<ApiResponse<IEnumerable<SrtParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SrtParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtParameterDto>> CreateAsync(CreateSrtParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtParameterDto>> UpdateAsync(long id, UpdateSrtParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

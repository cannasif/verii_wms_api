using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

public interface IPParameterService
{
    Task<ApiResponse<IEnumerable<PParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PParameterDto>> CreateAsync(CreatePParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PParameterDto>> UpdateAsync(long id, UpdatePParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

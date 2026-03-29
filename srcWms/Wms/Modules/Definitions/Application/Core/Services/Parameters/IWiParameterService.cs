using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

public interface IWiParameterService
{
    Task<ApiResponse<IEnumerable<WiParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiParameterDto>> CreateAsync(CreateWiParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiParameterDto>> UpdateAsync(long id, UpdateWiParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

public interface IWtParameterService
{
    Task<ApiResponse<IEnumerable<WtParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WtParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtParameterDto>> CreateAsync(CreateWtParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WtParameterDto>> UpdateAsync(long id, UpdateWtParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

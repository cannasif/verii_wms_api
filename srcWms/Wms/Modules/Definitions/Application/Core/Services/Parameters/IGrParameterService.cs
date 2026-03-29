using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

/// <summary>
/// `_old/reference/verii_wms_api.Application/Interfaces/Parameter/IGrParameterService.cs` karşılığıdır.
/// </summary>
public interface IGrParameterService
{
    Task<ApiResponse<IEnumerable<GrParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<GrParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrParameterDto>> CreateAsync(CreateGrParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<GrParameterDto>> UpdateAsync(long id, UpdateGrParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

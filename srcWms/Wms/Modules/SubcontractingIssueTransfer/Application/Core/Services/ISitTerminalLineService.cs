using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitTerminalLineService
{
    Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitTerminalLineDto>> CreateAsync(CreateSitTerminalLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitTerminalLineDto>> UpdateAsync(long id, UpdateSitTerminalLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}

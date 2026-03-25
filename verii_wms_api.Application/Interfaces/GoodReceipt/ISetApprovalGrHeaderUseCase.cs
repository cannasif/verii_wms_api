using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISetApprovalGrHeaderUseCase
    {
        Task<ApiResponse<GrHeaderDto>> ExecuteAsync(long id, bool approved, CancellationToken cancellationToken = default);
    }
}


using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ICompleteGrHeaderUseCase
    {
        Task<ApiResponse<bool>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
    }
}


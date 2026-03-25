using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IBulkCreateGrHeaderUseCase
    {
        Task<ApiResponse<long>> ExecuteAsync(BulkCreateGrRequestDto request, CancellationToken cancellationToken = default);
    }
}


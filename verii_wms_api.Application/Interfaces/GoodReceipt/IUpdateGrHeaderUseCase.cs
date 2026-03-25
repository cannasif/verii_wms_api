using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IUpdateGrHeaderUseCase
    {
        Task<ApiResponse<GrHeaderDto>> ExecuteAsync(int id, UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default);
    }
}


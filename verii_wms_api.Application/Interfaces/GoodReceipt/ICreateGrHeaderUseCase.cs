using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ICreateGrHeaderUseCase
    {
        Task<ApiResponse<GrHeaderDto>> ExecuteAsync(CreateGrHeaderDto createDto, CancellationToken cancellationToken = default);
    }
}


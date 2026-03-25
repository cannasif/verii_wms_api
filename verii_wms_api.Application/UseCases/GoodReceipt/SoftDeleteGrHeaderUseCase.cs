using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class SoftDeleteGrHeaderUseCase : ISoftDeleteGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public SoftDeleteGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<bool>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.SoftDeleteAsync(id, cancellationToken);
        }
    }
}


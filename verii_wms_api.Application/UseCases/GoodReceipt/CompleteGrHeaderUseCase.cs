using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class CompleteGrHeaderUseCase : ICompleteGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public CompleteGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<bool>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Behavior-preserving wrapper (risk-free boundary extraction).
            return _grHeaderService.CompleteAsync(id, cancellationToken);
        }
    }
}


using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class SetApprovalGrHeaderUseCase : ISetApprovalGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public SetApprovalGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<GrHeaderDto>> ExecuteAsync(long id, bool approved, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.SetApprovalAsync(id, approved, cancellationToken);
        }
    }
}


using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ICustomerMirrorService
    {
        Task<ApiResponse<PagedResponse<CustomerPagedDto>>> GetPagedAsync(PagedRequest request);
    }
}

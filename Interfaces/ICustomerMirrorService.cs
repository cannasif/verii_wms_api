using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ICustomerMirrorService
    {
        Task<ApiResponse<PagedResponse<CustomerPagedDto>>> GetPagedAsync(PagedRequest request);
    }
}

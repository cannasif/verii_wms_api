using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerMirrorController : ControllerBase
    {
        private readonly ICustomerMirrorService _service;

        public CustomerMirrorController(ICustomerMirrorService service)
        {
            _service = service;
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<CustomerPagedDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}

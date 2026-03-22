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
    public class StockMirrorController : ControllerBase
    {
        private readonly IStockMirrorService _service;

        public StockMirrorController(IStockMirrorService service)
        {
            _service = service;
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<StockPagedDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}

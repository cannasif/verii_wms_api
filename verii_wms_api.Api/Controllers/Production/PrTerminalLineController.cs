using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers.ProductionControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrTerminalLineController : ControllerBase
    {
        private readonly IPrTerminalLineService _prTerminalLineService;

        public PrTerminalLineController(IPrTerminalLineService prTerminalLineService)
        {
            _prTerminalLineService = prTerminalLineService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _prTerminalLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PrTerminalLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _prTerminalLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrTerminalLineDto>>> GetById(long id)
        {
            var result = await _prTerminalLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrTerminalLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _prTerminalLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrTerminalLineDto>>>> GetByUserId(long userId)
        {
            var result = await _prTerminalLineService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrTerminalLineDto>>> Create([FromBody] CreatePrTerminalLineDto createDto)
        {
            

            var result = await _prTerminalLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrTerminalLineDto>>> Update(long id, [FromBody] UpdatePrTerminalLineDto updateDto)
        {
            

            var result = await _prTerminalLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _prTerminalLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

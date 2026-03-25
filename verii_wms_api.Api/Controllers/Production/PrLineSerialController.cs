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
    public class PrLineSerialController : ControllerBase
    {
        private readonly IPrLineSerialService _prLineSerialService;

        public PrLineSerialController(IPrLineSerialService prLineSerialService)
        {
            _prLineSerialService = prLineSerialService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _prLineSerialService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PrLineSerialDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _prLineSerialService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PrLineSerialDto>>> GetById(long id)
        {
            var result = await _prLineSerialService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrLineSerialDto>>>> GetByLineId(long lineId)
        {
            var result = await _prLineSerialService.GetByLineIdAsync(lineId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrLineSerialDto>>> Create([FromBody] CreatePrLineSerialDto createDto)
        {
            

            var result = await _prLineSerialService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PrLineSerialDto>>> Update(long id, [FromBody] UpdatePrLineSerialDto updateDto)
        {
            

            var result = await _prLineSerialService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _prLineSerialService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

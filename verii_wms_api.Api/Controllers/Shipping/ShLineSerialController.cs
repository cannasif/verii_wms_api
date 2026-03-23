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
    public class ShLineSerialController : ControllerBase
    {
        private readonly IShLineSerialService _service;
        private readonly ILocalizationService _localizationService;

        public ShLineSerialController(IShLineSerialService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<ShLineSerialDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _service.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ShLineSerialDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("line/{lineId}/paged")]
        public async Task<IActionResult> GetByLineId(long lineId, [FromBody] PagedRequest request)
        {
            var result = await _service.GetByLineIdAsync(lineId);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ShLineSerialDto>>> Create([FromBody] CreateShLineSerialDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ShLineSerialDto>>> Update(long id, [FromBody] UpdateShLineSerialDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

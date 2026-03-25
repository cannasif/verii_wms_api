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
    public class SitLineSerialController : ControllerBase
    {
        private readonly ISitLineSerialService _service;
        private readonly ILocalizationService _localizationService;

        public SitLineSerialController(ISitLineSerialService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SitLineSerialDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SitLineSerialDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("line/{lineId}/paged")]
        public async Task<IActionResult> GetByLineId(long lineId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SitLineSerialDto>>> Create([FromBody] CreateSitLineSerialDto createDto, CancellationToken cancellationToken = default)
        {
            
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SitLineSerialDto>>> Update(long id, [FromBody] UpdateSitLineSerialDto updateDto, CancellationToken cancellationToken = default)
        {
            
            var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

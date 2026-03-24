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
    public class WiParameterController : ControllerBase
    {
        private readonly IWiParameterService _wiParameterService;
        private readonly ILocalizationService _localizationService;

        public WiParameterController(IWiParameterService wiParameterService, ILocalizationService localizationService)
        {
            _wiParameterService = wiParameterService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wiParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WiParameterDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wiParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<WiParameterDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wiParameterService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WiParameterDto>>> Create([FromBody] CreateWiParameterDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _wiParameterService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<WiParameterDto>>> Update(long id, [FromBody] UpdateWiParameterDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _wiParameterService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wiParameterService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}


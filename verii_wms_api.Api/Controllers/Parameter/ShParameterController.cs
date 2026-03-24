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
    public class ShParameterController : ControllerBase
    {
        private readonly IShParameterService _shParameterService;
        private readonly ILocalizationService _localizationService;

        public ShParameterController(IShParameterService shParameterService, ILocalizationService localizationService)
        {
            _shParameterService = shParameterService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _shParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<ShParameterDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _shParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<ShParameterDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _shParameterService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ShParameterDto>>> Create([FromBody] CreateShParameterDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ShParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _shParameterService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<ShParameterDto>>> Update(long id, [FromBody] UpdateShParameterDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<ShParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _shParameterService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _shParameterService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}


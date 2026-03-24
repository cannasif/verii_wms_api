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
    public class PrParameterController : ControllerBase
    {
        private readonly IPrParameterService _prParameterService;
        private readonly ILocalizationService _localizationService;

        public PrParameterController(IPrParameterService prParameterService, ILocalizationService localizationService)
        {
            _prParameterService = prParameterService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _prParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PrParameterDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _prParameterService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<PrParameterDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _prParameterService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrParameterDto>>> Create([FromBody] CreatePrParameterDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PrParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _prParameterService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<PrParameterDto>>> Update(long id, [FromBody] UpdatePrParameterDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<PrParameterDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _prParameterService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _prParameterService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}


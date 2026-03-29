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
    public class PLineController : ControllerBase
    {
        private readonly IPLineService _pLineService;
        private readonly ILocalizationService _localizationService;

        public PLineController(IPLineService pLineService, ILocalizationService localizationService)
        {
            _pLineService = pLineService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _pLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PLineDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _pLineService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PLineDto?>>> GetById(long id)
        {
            var result = await _pLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("package/{packageId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PLineDto>>>> GetByPackageId(long packageId)
        {
            var result = await _pLineService.GetByPackageIdAsync(packageId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{packingHeaderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PLineDto>>>> GetByPackingHeaderId(long packingHeaderId)
        {
            var result = await _pLineService.GetByPackingHeaderIdAsync(packingHeaderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PLineDto>>> Create([FromBody] CreatePLineDto createDto)
        {
            

            var result = await _pLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PLineDto>>> Update(long id, [FromBody] UpdatePLineDto updateDto)
        {
            

            var result = await _pLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _pLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}


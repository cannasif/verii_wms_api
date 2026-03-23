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
    public class GrLineSerialController : ControllerBase
    {
        private readonly IGrLineSerialService _grLineSerialService;
        private readonly ILocalizationService _localizationService;

        public GrLineSerialController(IGrLineSerialService grLineSerialService, ILocalizationService localizationService)
        {
            _grLineSerialService = grLineSerialService;
            _localizationService = localizationService;
        }

        [HttpPost("paged")]
        public async Task<IActionResult> Get([FromBody] PagedRequest request)
        {
            var result = await _grLineSerialService.GetPagedAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> GetById(long id)
        {
            var result = await _grLineSerialService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("by-line/{lineId}/paged")]
        public async Task<IActionResult> GetByLineId(long lineId, [FromBody] PagedRequest request)
        {
            var result = await _grLineSerialService.GetByLineIdAsync(lineId);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> Create([FromBody] CreateGrLineSerialDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }
            var result = await _grLineSerialService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> Update(long id, [FromBody] UpdateGrLineSerialDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }
            var result = await _grLineSerialService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _grLineSerialService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        
    }
}

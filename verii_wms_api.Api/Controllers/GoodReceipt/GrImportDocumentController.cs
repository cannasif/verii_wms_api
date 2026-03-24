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
    public class GrImportDocumentController : ControllerBase
    {
        private readonly IGrImportDocumentService _grImportDocumentService;
        private readonly ILocalizationService _localizationService;

        public GrImportDocumentController(IGrImportDocumentService grImportDocumentService, ILocalizationService localizationService)
        {
            _grImportDocumentService = grImportDocumentService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grImportDocumentService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grImportDocumentService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportDocumentDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _grImportDocumentService.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> Create([FromBody] CreateGrImportDocumentDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grImportDocumentService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> Update(long id, [FromBody] UpdateGrImportDocumentDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grImportDocumentService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grImportDocumentService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrImportDocumentDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grImportDocumentService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

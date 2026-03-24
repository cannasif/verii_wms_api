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
    public class SrtImportLineController : ControllerBase
    {
        private readonly ISrtImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public SrtImportLineController(ISrtImportLineService service, ILocalizationService localizationService)
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
        public async Task<ActionResult<ApiResponse<PagedResponse<SrtImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }




        [HttpPost]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Create([FromBody] CreateSrtImportLineDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Update(long id, [FromBody] UpdateSrtImportLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddSrtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("subcontractingReceiptTransferOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>>> SubcontractingReceiptTransferOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

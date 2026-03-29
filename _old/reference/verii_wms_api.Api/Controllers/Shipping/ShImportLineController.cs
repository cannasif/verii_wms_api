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
    public class ShImportLineController : ControllerBase
    {
        private readonly IShImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public ShImportLineController(IShImportLineService service, ILocalizationService localizationService)
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
        public async Task<ActionResult<ApiResponse<PagedResponse<ShImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ShImportLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<ShImportLineDto>>> Create([FromBody] CreateShImportLineDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ShImportLineDto>>> Update(long id, [FromBody] UpdateShImportLineDto updateDto, CancellationToken cancellationToken = default)
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

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<ActionResult<ApiResponse<ShImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddShImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("shipmentOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>>> ShipmentOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

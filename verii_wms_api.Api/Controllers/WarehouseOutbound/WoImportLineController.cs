using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WoImportLineController : ControllerBase
    {
        private readonly IWoImportLineService _service;
        private readonly ILocalizationService _localizationService;

        public WoImportLineController(IWoImportLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<IActionResult> GetByLineId(long lineId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<IActionResult> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }





        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWoImportLineDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateWoImportLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<IActionResult> AddBarcodeBasedonAssignedOrder([FromBody] AddWoImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("warehouseOutboundOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WoImportLineWithRoutesDto>>>> WarehouseOutboundOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

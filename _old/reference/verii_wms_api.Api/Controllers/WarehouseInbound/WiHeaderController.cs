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
    public class WiHeaderController : ControllerBase
    {
        private readonly IWiHeaderService _service;
        private readonly ILocalizationService _localizationService;

        public WiHeaderController(IWiHeaderService service, ILocalizationService localizationService)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        
        

        [HttpGet("inbound/{inboundType}")]
        public async Task<IActionResult> GetByInboundType(string inboundType, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByInboundTypeAsync(inboundType, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("account/{accountCode}")]
        public async Task<IActionResult> GetByAccountCode(string accountCode, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByAccountCodeAsync(accountCode, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWiHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("assigned/{userId}/paged")]
        public async Task<IActionResult> GetAssignedOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAssignedOrdersAsync(userId, cancellationToken);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpGet("assigned-lines/{headerId}")]
        public async Task<ActionResult<ApiResponse<WiAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAssignedOrderLinesAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateWiHeaderDto updateDto, CancellationToken cancellationToken = default)
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

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.CompleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<WiHeaderDto>>> Generate([FromBody] GenerateWarehouseInboundOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GenerateWarehouseInboundOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk-create")]
        public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkCreateWiRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.BulkCreateWarehouseInboundAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WiHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<WiHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
        {
            var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

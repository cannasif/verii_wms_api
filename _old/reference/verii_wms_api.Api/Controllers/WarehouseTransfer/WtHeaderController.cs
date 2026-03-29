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
    public class WtHeaderController : ControllerBase
    {
        private readonly IWtHeaderService _wtHeaderService;

        public WtHeaderController(IWtHeaderService wtHeaderService)
        {
            _wtHeaderService = wtHeaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        


        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> Create([FromBody] CreateWtHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _wtHeaderService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> Update(long id, [FromBody] UpdateWtHeaderDto updateDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _wtHeaderService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.CompleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("assigned/{userId}/paged")]
        public async Task<IActionResult> GetAssignedTransferOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetAssignedTransferOrdersAsync(userId, request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAssignedTransferOrderLines/{headerId}")]
        public async Task<ActionResult<ApiResponse<WtAssignedTransferOrderLinesDto>>> GetAssignedTransferOrderLines(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetAssignedTransferOrderLinesAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> GenerateWarehouseTransferOrder([FromBody] GenerateWarehouseTransferOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            

            var result = await _wtHeaderService.GenerateWarehouseTransferOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk-generate")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> BulkWtGenerate([FromBody] BulkWtGenerateRequestDto request, CancellationToken cancellationToken = default)
        {
            
            var result = await _wtHeaderService.BulkWtGenerateAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<WtHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
        {
            var result = await _wtHeaderService.SetApprovalAsync(id, approved, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

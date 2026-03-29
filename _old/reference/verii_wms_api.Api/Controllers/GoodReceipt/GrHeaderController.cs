using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrHeaderController : ControllerBase
    {
        private readonly IGrHeaderService _grHeaderService;

        public GrHeaderController(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto?>>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Create([FromBody] CreateGrHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _grHeaderService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Update(int id, [FromBody] UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _grHeaderService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.CompleteAsync((int)id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("assigned/{userId}/paged")]
        public async Task<IActionResult> GetAssignedOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetAssignedOrdersAsync(userId, cancellationToken);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpGet("getAssignedOrderLines/{headerId}")]
        public async Task<ActionResult<ApiResponse<GrAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetAssignedOrderLinesAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.SetApprovalAsync(id, approved, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetByCustomerCode(string customerCode, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetByCustomerCodeAsync(customerCode, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grHeaderService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("bulkCreate")]
        public async Task<ActionResult<ApiResponse<long>>> BulkCreate([FromBody] BulkCreateGrRequestDto request, CancellationToken cancellationToken = default)
        {

            var result = await _grHeaderService.BulkCreateAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<GrHeaderDto>>> GenerateOrder([FromBody] GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default)
        {

            var result = await _grHeaderService.GenerateGoodReceiptOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        
    }
}

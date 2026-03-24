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
    public class SitHeaderController : ControllerBase
    {
        private readonly ISitHeaderService _service;
        private readonly ILocalizationService _localizationService;

        public SitHeaderController(ISitHeaderService service, ILocalizationService localizationService)
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
        public async Task<ActionResult<ApiResponse<PagedResponse<SitHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SitHeaderDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        


        [HttpGet("customer/{customerCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SitHeaderDto>>>> GetByCustomerCode(string customerCode, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByCustomerCodeAsync(customerCode, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("doctype/{documentType}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SitHeaderDto>>>> GetByDocumentType(string documentType, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByDocumentTypeAsync(documentType, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("docno/{documentNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SitHeaderDto>>>> GetByDocumentNo(string documentNo, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByDocumentNoAsync(documentNo, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SitHeaderDto>>> Create([FromBody] CreateSitHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SitHeaderDto>>> Update(long id, [FromBody] UpdateSitHeaderDto updateDto, CancellationToken cancellationToken = default)
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

        [HttpPost("complete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Complete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.CompleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("assigned/{userId}/paged")]
        public async Task<IActionResult> GetAssignedOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAssignedOrdersAsync(userId, cancellationToken);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        [HttpGet("getAssignedOrderLines/{headerId}")]
        public async Task<ActionResult<ApiResponse<SitAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAssignedOrderLinesAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ApiResponse<SitHeaderDto>>> Generate([FromBody] GenerateSubcontractingIssueOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GenerateOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk-create")]
        public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkCreateSitRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _service.BulkCreateSubcontractingIssueTransferAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("completed-awaiting-erp-approval")]
        public async Task<ActionResult<ApiResponse<PagedResponse<SitHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("approval/{id}")]
        public async Task<ActionResult<ApiResponse<SitHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
        {
            var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

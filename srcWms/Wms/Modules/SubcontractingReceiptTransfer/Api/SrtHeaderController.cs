using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Application.SubcontractingReceiptTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingReceiptTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SrtHeaderController : ControllerBase
{
    private readonly ISrtHeaderService _service;

    public SrtHeaderController(ISrtHeaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSrtHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSrtHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> SoftDelete(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("complete/{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> Complete(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("assigned/{userId:long}/paged")]
    public async Task<IActionResult> GetAssignedOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedSubcontractingReceiptTransferOrdersPagedAsync(userId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("assigned-lines/{headerId:long}")]
    [HttpGet("getAssignedOrderLines/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<SrtAssignedSubcontractingReceiptTransferOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedOrderLinesAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> Generate([FromBody] GenerateSubcontractingReceiptTransferOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateSubcontractingReceiptTransferOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("process")]
    public async Task<ActionResult<ApiResponse<int>>> Process([FromBody] BulkSrtGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ProcessSubcontractingReceiptTransferAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("bulk-create")]
    public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkSrtGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.BulkCreateSubcontractingReceiptTransferAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("completed-awaiting-erp-approval")]
    public async Task<ActionResult<ApiResponse<PagedResponse<SrtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("approval/{id:long}")]
    public async Task<ActionResult<ApiResponse<SrtHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

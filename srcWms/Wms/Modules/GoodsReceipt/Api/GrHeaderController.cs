using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Application.GoodsReceipt.Services;

namespace Wms.Modules.GoodsReceipt.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class GrHeaderController : ControllerBase
{
    private readonly IGrHeaderService _service;

    public GrHeaderController(IGrHeaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GrHeaderDto?>>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Create([FromBody] CreateGrHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<GrHeaderDto>>> Update(int id, [FromBody] UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("bulkCreate")]
    public async Task<ActionResult<ApiResponse<long>>> BulkCreate([FromBody] BulkCreateGrRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.BulkCreateAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("process")]
    public async Task<ActionResult<ApiResponse<long>>> Process([FromBody] ProcessGrRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ProcessGoodsReceiptAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<GrHeaderDto>>> GenerateOrder([FromBody] GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateGoodReceiptOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("assigned/{userId:long}/paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetAssignedOrders(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedOrdersPagedAsync(userId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("getAssignedOrderLines/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<GrAssignedOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedOrderLinesAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("completed-awaiting-erp-approval")]
    public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("approval/{id:long}")]
    public async Task<ActionResult<ApiResponse<GrHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("complete/{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Complete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-customer/{customerCode}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GrHeaderDto>>>> GetByCustomerCode(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByCustomerCodeAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<GrHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Application.ProductionTransfer.Services;

namespace Wms.WebApi.Controllers.ProductionTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PtHeaderController : ControllerBase
{
    private readonly IPtHeaderService _service;

    public PtHeaderController(IPtHeaderService service)
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
    public async Task<IActionResult> Create([FromBody] CreatePtHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdatePtHeaderDto updateDto, CancellationToken cancellationToken = default)
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
        var result = await _service.GetAssignedProductionTransferOrdersPagedAsync(userId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("assigned-lines/{headerId:long}")]
    [HttpGet("getAssignedProductionTransferOrderLines/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<PtAssignedProductionTransferOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedProductionTransferOrderLinesAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("production-transfer-suggestions")]
    public async Task<ActionResult<ApiResponse<List<ProductionTransferSuggestedLineDto>>>> GetProductionTransferSuggestions([FromBody] ProductionTransferSuggestionRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductionTransferSuggestionsAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("production-transfer/{id:long}")]
    public async Task<ActionResult<ApiResponse<ProductionTransferDetailDto>>> GetProductionTransferDetail(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductionTransferDetailAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("production-transfer")]
    public async Task<ActionResult<ApiResponse<PtHeaderDto>>> CreateProductionTransfer([FromBody] CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateProductionTransferAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("production-transfer/{id:long}")]
    public async Task<ActionResult<ApiResponse<PtHeaderDto>>> UpdateProductionTransfer(long id, [FromBody] CreateProductionTransferRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateProductionTransferAsync(id, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<PtHeaderDto>>> Generate([FromBody] GenerateProductionTransferOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateProductionTransferOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("bulk-create")]
    [HttpPost("bulk-generate")]
    public async Task<ActionResult<ApiResponse<PtHeaderDto>>> BulkCreate([FromBody] BulkPtGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.BulkPtGenerateAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("completed-awaiting-erp-approval")]
    public async Task<ActionResult<ApiResponse<PagedResponse<PtHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("approval/{id:long}")]
    public async Task<ActionResult<ApiResponse<PtHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

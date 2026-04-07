using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.Production.Services;

namespace Wms.WebApi.Controllers.Production;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PrHeaderController : ControllerBase
{
    private readonly IPrHeaderService _service;

    public PrHeaderController(IPrHeaderService service)
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

    [HttpGet("detail/{id:long}")]
    public async Task<IActionResult> GetDetail(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetDetailAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("paged")]
    public async Task<IActionResult> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdatePrHeaderDto updateDto, CancellationToken cancellationToken = default)
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
        var result = await _service.GetAssignedProductionOrdersPagedAsync(userId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("assigned-lines/{headerId:long}")]
    [HttpGet("getAssignedProductionOrderLines/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<PrAssignedProductionOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedProductionOrderLinesAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("plan")]
    public async Task<ActionResult<ApiResponse<PrHeaderDto>>> CreatePlan([FromBody] CreateProductionPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreatePlanAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("plan/{id:long}")]
    public async Task<ActionResult<ApiResponse<PrHeaderDto>>> UpdatePlan(long id, [FromBody] CreateProductionPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdatePlanAsync(id, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("erp-template")]
    public async Task<ActionResult<ApiResponse<ProductionPlanDraftDto>>> GetErpTemplate([FromBody] ProductionErpTemplateRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetErpTemplateAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<PrHeaderDto>>> Generate([FromBody] GenerateProductionOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateProductionOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("bulk-create")]
    [HttpPost("bulk-generate")]
    public async Task<ActionResult<ApiResponse<PrHeaderDto>>> BulkCreate([FromBody] BulkPrGenerateRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.BulkPrGenerateAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("completed-awaiting-erp-approval")]
    public async Task<ActionResult<ApiResponse<PagedResponse<PrHeaderDto>>>> GetCompletedAwaitingErpApproval([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCompletedAwaitingErpApprovalPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("approval/{id:long}")]
    public async Task<ActionResult<ApiResponse<PrHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

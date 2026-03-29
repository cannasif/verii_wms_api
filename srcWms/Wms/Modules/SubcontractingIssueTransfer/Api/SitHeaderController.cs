using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Application.SubcontractingIssueTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingIssueTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SitHeaderController : ControllerBase
{
    private readonly ISitHeaderService _service;

    public SitHeaderController(ISitHeaderService service)
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
    public async Task<IActionResult> Create([FromBody] CreateSitHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSitHeaderDto updateDto, CancellationToken cancellationToken = default)
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
        var result = await _service.GetAssignedSubcontractingIssueTransferOrdersAsync(userId, cancellationToken);
        var items = result.Data?.ToList() ?? new List<SitHeaderDto>();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var pagedData = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var pagedResult = result.Success
            ? ApiResponse<PagedResponse<SitHeaderDto>>.SuccessResult(new PagedResponse<SitHeaderDto>(pagedData, items.Count, pageNumber, pageSize), result.Message)
            : ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
        return StatusCode(pagedResult.StatusCode, pagedResult);
    }

    [HttpGet("assigned-lines/{headerId:long}")]
    [HttpGet("getAssignedOrderLines/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<SitAssignedSubcontractingIssueTransferOrderLinesDto>>> GetAssignedOrderLines(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAssignedOrderLinesAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<SitHeaderDto>>> Generate([FromBody] GenerateSubcontractingIssueTransferOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateSubcontractingIssueTransferOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("bulk-create")]
    public async Task<ActionResult<ApiResponse<int>>> BulkCreate([FromBody] BulkSitGenerateRequestDto request, CancellationToken cancellationToken = default)
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

    [HttpPost("approval/{id:long}")]
    public async Task<ActionResult<ApiResponse<SitHeaderDto>>> SetApproval(long id, [FromQuery] bool approved, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetApprovalAsync(id, approved, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

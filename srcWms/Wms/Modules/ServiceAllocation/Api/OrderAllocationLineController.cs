using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Application.ServiceAllocation.Services;

namespace Wms.WebApi.Controllers.ServiceAllocation;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class OrderAllocationLineController : ControllerBase
{
    private readonly IOrderAllocationLineService _service;

    public OrderAllocationLineController(IOrderAllocationLineService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderAllocationLineDto>>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<OrderAllocationLineDto>>>> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<OrderAllocationLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderAllocationLineDto>>> Create([FromBody] CreateOrderAllocationLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<OrderAllocationLineDto>>> Update(long id, [FromBody] UpdateOrderAllocationLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<OrderAllocationLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("recompute")]
    public async Task<ActionResult<ApiResponse<AllocationRecomputeResultDto>>> Recompute([FromBody] RecomputeAllocationRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.RecomputeAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

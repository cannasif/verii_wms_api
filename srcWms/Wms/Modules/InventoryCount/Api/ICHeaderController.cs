using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcHeaderController : ControllerBase
{
    private readonly IIcHeaderService _service;
    public IcHeaderController(IIcHeaderService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<IcHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost("assigned/{userId:long}/paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<IcHeaderDto>>>> GetAssignedPaged(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetAssignedPagedAsync(userId, request, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<IcHeaderDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IcHeaderDto>>> Create([FromBody] CreateIcHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<IcHeaderDto>>> Update(long id, [FromBody] UpdateIcHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost("{id:long}/generate-lines")]
    public async Task<ActionResult<ApiResponse<bool>>> GenerateLines(long id, CancellationToken cancellationToken = default)
    {
        var r = await _service.GenerateLinesAsync(id, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
    {
        var r = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcRouteController : ControllerBase
{
    private readonly IIcRouteService _service;
    public IcRouteController(IIcRouteService service) => _service = service;
    [HttpGet] public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("paged")] public async Task<ActionResult<ApiResponse<PagedResponse<IcRouteDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("{id:long}")] public async Task<ActionResult<ApiResponse<IcRouteDto>>> GetById(long id, CancellationToken cancellationToken = default) { var r = await _service.GetByIdAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("importline/{importLineId:long}")] public async Task<ActionResult<ApiResponse<IEnumerable<IcRouteDto>>>> GetByImportLineId(long importLineId, CancellationToken cancellationToken = default) { var r = await _service.GetByImportLineIdAsync(importLineId, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost] public async Task<ActionResult<ApiResponse<IcRouteDto>>> Create([FromBody] CreateIcRouteDto createDto, CancellationToken cancellationToken = default) { var r = await _service.CreateAsync(createDto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPut("{id:long}")] public async Task<ActionResult<ApiResponse<IcRouteDto>>> Update(long id, [FromBody] UpdateIcRouteDto updateDto, CancellationToken cancellationToken = default) { var r = await _service.UpdateAsync(id, updateDto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpDelete("{id:long}")] public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default) { var r = await _service.SoftDeleteAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
}

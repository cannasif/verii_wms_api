using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcScopeController : ControllerBase
{
    private readonly IIcScopeService _service;
    public IcScopeController(IIcScopeService service) => _service = service;

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<IcScopeDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IcScopeDto>>> Create([FromBody] CreateIcScopeDto createDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<IcScopeDto>>> Update(long id, [FromBody] UpdateIcScopeDto updateDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
    {
        var r = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }
}

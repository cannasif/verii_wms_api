using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcLineController : ControllerBase
{
    private readonly IIcLineService _service;
    public IcLineController(IIcLineService service) => _service = service;

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<IcLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<IcLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IcLineDto>>> Create([FromBody] CreateIcLineDto createDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<IcLineDto>>> Update(long id, [FromBody] UpdateIcLineDto updateDto, CancellationToken cancellationToken = default)
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

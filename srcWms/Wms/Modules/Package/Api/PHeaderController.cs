using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Package.Dtos;
using Wms.Application.Package.Services;

namespace Wms.WebApi.Controllers.Package;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PHeaderController : ControllerBase
{
    private readonly IPHeaderService _service;

    public PHeaderController(IPHeaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<PHeaderDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<PHeaderDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PHeaderDto>>> Create([FromBody] CreatePHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<PHeaderDto>>> Update(long id, [FromBody] UpdatePHeaderDto updateDto, CancellationToken cancellationToken = default)
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

    [HttpGet("available-for-mapping/{sourceType}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetAvailableHeadersForMapping(string sourceType, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAvailableHeadersForMappingAsync(sourceType, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{pHeaderId:long}/match-plines")]
    public async Task<ActionResult<ApiResponse<bool>>> MatchPlinesWithMatchedStatus(long pHeaderId, [FromBody] MatchPlinesRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.MatchPlinesWithMatchedStatus(pHeaderId, request.IsMatched, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Application.SubcontractingIssueTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingIssueTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SitTerminalLineController : ControllerBase
{
    private readonly ISitTerminalLineService _service;

    public SitTerminalLineController(ISitTerminalLineService service)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<SitTerminalLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<SitTerminalLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SitTerminalLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SitTerminalLineDto>>>> GetByUserId(long userId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByUserIdAsync(userId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SitTerminalLineDto>>> Create([FromBody] CreateSitTerminalLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<SitTerminalLineDto>>> Update(long id, [FromBody] UpdateSitTerminalLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

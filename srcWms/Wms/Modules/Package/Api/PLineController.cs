using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Package.Dtos;
using Wms.Application.Package.Services;

namespace Wms.WebApi.Controllers.Package;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PLineController : ControllerBase
{
    private readonly IPLineService _service;

    public PLineController(IPLineService service)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<PLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<PLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("package/{packageId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PLineDto>>>> GetByPackageId(long packageId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPackageIdAsync(packageId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("header/{packingHeaderId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PLineDto>>>> GetByPackingHeaderId(long packingHeaderId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPackingHeaderIdAsync(packingHeaderId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PLineDto>>> Create([FromBody] CreatePLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<PLineDto>>> Update(long id, [FromBody] UpdatePLineDto updateDto, CancellationToken cancellationToken = default)
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
}

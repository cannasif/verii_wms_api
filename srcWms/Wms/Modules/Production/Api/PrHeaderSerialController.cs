using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.Production.Services;

namespace Wms.WebApi.Controllers.Production;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PrHeaderSerialController : ControllerBase
{
    private readonly IPrHeaderSerialService _service; public PrHeaderSerialController(IPrHeaderSerialService service) => _service = service;
    [HttpGet] public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("paged")] public async Task<ActionResult<ApiResponse<PagedResponse<PrHeaderSerialDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("{id:long}")] public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> GetById(long id, CancellationToken cancellationToken = default) { var r = await _service.GetByIdAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("header/{headerId:long}")] public async Task<ActionResult<ApiResponse<IEnumerable<PrHeaderSerialDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default) { var r = await _service.GetByHeaderIdAsync(headerId, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost] public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> Create([FromBody] CreatePrHeaderSerialDto dto, CancellationToken cancellationToken = default) { var r = await _service.CreateAsync(dto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPut("{id:long}")] public async Task<ActionResult<ApiResponse<PrHeaderSerialDto>>> Update(long id, [FromBody] UpdatePrHeaderSerialDto dto, CancellationToken cancellationToken = default) { var r = await _service.UpdateAsync(id, dto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpDelete("{id:long}")] public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default) { var r = await _service.SoftDeleteAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
}

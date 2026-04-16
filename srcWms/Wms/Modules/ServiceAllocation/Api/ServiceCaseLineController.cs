using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Application.ServiceAllocation.Services;

namespace Wms.WebApi.Controllers.ServiceAllocation;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ServiceCaseLineController : ControllerBase
{
    private readonly IServiceCaseLineService _service;

    public ServiceCaseLineController(IServiceCaseLineService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ServiceCaseLineDto>>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<ServiceCaseLineDto>>>> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<ServiceCaseLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ServiceCaseLineDto>>> Create([FromBody] CreateServiceCaseLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<ServiceCaseLineDto>>> Update(long id, [FromBody] UpdateServiceCaseLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<ServiceCaseLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

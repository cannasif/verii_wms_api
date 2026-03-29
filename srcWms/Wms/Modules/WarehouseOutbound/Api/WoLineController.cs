using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Application.WarehouseOutbound.Services;

namespace Wms.WebApi.Controllers.WarehouseOutbound;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WoLineController : ControllerBase
{
    private readonly IWoLineService _service;

    public WoLineController(IWoLineService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<WoLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("by-header/{headerId:long}/paged")]
    public async Task<IActionResult> GetByHeaderId(long headerId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        var items = result.Data?.ToList() ?? new List<WoLineDto>();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var pagedData = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var pagedResult = result.Success
            ? ApiResponse<PagedResponse<WoLineDto>>.SuccessResult(new PagedResponse<WoLineDto>(pagedData, items.Count, pageNumber, pageSize), result.Message)
            : ApiResponse<PagedResponse<WoLineDto>>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
        return StatusCode(pagedResult.StatusCode, pagedResult);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WoLineDto>>> Create([FromBody] CreateWoLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<WoLineDto>>> Update(long id, [FromBody] UpdateWoLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<WoLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

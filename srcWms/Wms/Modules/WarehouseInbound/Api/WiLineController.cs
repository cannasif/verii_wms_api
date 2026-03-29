using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Application.WarehouseInbound.Services;

namespace Wms.WebApi.Controllers.WarehouseInbound;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WiLineController : ControllerBase
{
    private readonly IWiLineService _service;

    public WiLineController(IWiLineService service)
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
    public async Task<ActionResult<ApiResponse<WiLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("by-header/{headerId:long}/paged")]
    public async Task<IActionResult> GetByHeaderId(long headerId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        var items = result.Data?.ToList() ?? new List<WiLineDto>();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var pagedData = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var pagedResult = result.Success
            ? ApiResponse<PagedResponse<WiLineDto>>.SuccessResult(new PagedResponse<WiLineDto>(pagedData, items.Count, pageNumber, pageSize), result.Message)
            : ApiResponse<PagedResponse<WiLineDto>>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
        return StatusCode(pagedResult.StatusCode, pagedResult);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WiLineDto>>> Create([FromBody] CreateWiLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<WiLineDto>>> Update(long id, [FromBody] UpdateWiLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<WiLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

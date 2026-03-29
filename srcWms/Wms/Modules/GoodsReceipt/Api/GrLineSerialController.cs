using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Application.GoodsReceipt.Services;

namespace Wms.Modules.GoodsReceipt.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class GrLineSerialController : ControllerBase
{
    private readonly IGrLineSerialService _service;

    public GrLineSerialController(IGrLineSerialService service)
    {
        _service = service;
    }

    [HttpPost("paged")]
    public async Task<IActionResult> Get([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("by-line/{lineId:long}/paged")]
    public async Task<IActionResult> GetByLineId(long lineId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        var items = result.Data?.ToList() ?? new List<GrLineSerialDto>();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var pagedData = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var pagedResult = result.Success
            ? ApiResponse<PagedResponse<GrLineSerialDto>>.SuccessResult(new PagedResponse<GrLineSerialDto>(pagedData, items.Count, pageNumber, pageSize), result.Message)
            : ApiResponse<PagedResponse<GrLineSerialDto>>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
        return StatusCode(pagedResult.StatusCode, pagedResult);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> Create([FromBody] CreateGrLineSerialDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<GrLineSerialDto>>> Update(long id, [FromBody] UpdateGrLineSerialDto updateDto, CancellationToken cancellationToken = default)
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

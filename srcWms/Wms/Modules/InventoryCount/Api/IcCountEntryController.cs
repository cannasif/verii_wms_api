using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcCountEntryController : ControllerBase
{
    private readonly IIcCountEntryService _service;
    public IcCountEntryController(IIcCountEntryService service) => _service = service;

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<IcCountEntryDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var r = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IcCountEntryDto>>> Create([FromBody] CreateIcCountEntryDto createDto, CancellationToken cancellationToken = default)
    {
        var r = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(r.StatusCode, r);
    }
}

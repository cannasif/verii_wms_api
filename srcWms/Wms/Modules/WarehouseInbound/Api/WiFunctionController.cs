using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Application.WarehouseInbound.Services;

namespace Wms.WebApi.Controllers.WarehouseInbound;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WiFunctionController : ControllerBase
{
    private readonly IWiFunctionService _service;

    public WiFunctionController(IWiFunctionService service)
    {
        _service = service;
    }

    [HttpGet("headers/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<WiOpenOrderHeaderDto>>>> GetWiOpenOrderHeader(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetWiOpenOrderHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/{siparisNoCsv}")]
    public async Task<ActionResult<ApiResponse<List<WiOpenOrderLineDto>>>> GetWiOpenOrderLine(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetWiOpenOrderLineAsync(siparisNoCsv, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Application.Shipping.Services;

namespace Wms.WebApi.Controllers.Shipping;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ShFunctionController : ControllerBase
{
    private readonly IShFunctionService _service;

    public ShFunctionController(IShFunctionService service)
    {
        _service = service;
    }

    [HttpGet("headers/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<TransferOpenOrderHeaderDto>>>> GetShippingOpenOrderHeader(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetShippingOpenOrderHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/{siparisNoCsv}")]
    public async Task<ActionResult<ApiResponse<List<TransferOpenOrderLineDto>>>> GetShippingOpenOrderLine(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetShippingOpenOrderLineAsync(siparisNoCsv, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

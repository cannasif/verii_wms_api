using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Application.SubcontractingIssueTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingIssueTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SitFunctionController : ControllerBase
{
    private readonly ISitFunctionService _service;

    public SitFunctionController(ISitFunctionService service)
    {
        _service = service;
    }

    [HttpGet("headers/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<SitOpenOrderHeaderDto>>>> GetSitOpenOrderHeader(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSitOpenOrderHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/{siparisNoCsv}")]
    public async Task<ActionResult<ApiResponse<List<SitOpenOrderLineDto>>>> GetSitOpenOrderLine(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSitOpenOrderLineAsync(siparisNoCsv, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

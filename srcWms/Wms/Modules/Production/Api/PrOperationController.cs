using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.Production.Services;

namespace Wms.WebApi.Controllers.Production;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PrOperationController : ControllerBase
{
    private readonly IPrOperationService _service;

    public PrOperationController(IPrOperationService service)
    {
        _service = service;
    }

    [HttpPost("start")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> Start([FromBody] StartPrOperationRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.StartAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{operationId:long}/pause")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> Pause(long operationId, [FromBody] PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.PauseAsync(operationId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{operationId:long}/resume")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> Resume(long operationId, [FromBody] PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ResumeAsync(operationId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{operationId:long}/consumption")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> AddConsumption(long operationId, [FromBody] AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddConsumptionAsync(operationId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{operationId:long}/output")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> AddOutput(long operationId, [FromBody] AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddOutputAsync(operationId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{operationId:long}/complete")]
    public async Task<ActionResult<ApiResponse<PrOperationDto>>> Complete(long operationId, [FromBody] PrOperationEventRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(operationId, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;

namespace Wms.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BarcodeController : ControllerBase
{
    private readonly IBarcodeDefinitionService _definitions;
    private readonly IBarcodeResolutionService _resolution;
    private readonly ILocalizationService _localizationService;

    public BarcodeController(
        IBarcodeDefinitionService definitions,
        IBarcodeResolutionService resolution,
        ILocalizationService localizationService)
    {
        _definitions = definitions;
        _resolution = resolution;
        _localizationService = localizationService;
    }

    [HttpGet("definitions")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BarcodeDefinitionDto>>>> GetDefinitions(CancellationToken cancellationToken = default)
    {
        var data = await _definitions.GetDefinitionsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<BarcodeDefinitionDto>>.SuccessResult(
            data,
            _localizationService.GetLocalizedString("BarcodeDefinitionsRetrievedSuccessfully")));
    }

    [HttpGet("definitions/{moduleKey}")]
    public async Task<ActionResult<ApiResponse<BarcodeDefinitionDto>>> GetDefinition(string moduleKey, CancellationToken cancellationToken = default)
    {
        var definition = await _definitions.GetDefinitionAsync(moduleKey, cancellationToken);
        if (definition == null)
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionNotFound");
            return StatusCode(404, ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 404));
        }

        return Ok(ApiResponse<BarcodeDefinitionDto>.SuccessResult(
            definition,
            _localizationService.GetLocalizedString("BarcodeDefinitionsRetrievedSuccessfully")));
    }

    [HttpPost("definitions")]
    public async Task<ActionResult<ApiResponse<BarcodeDefinitionDto>>> CreateDefinition(
        [FromBody] SaveBarcodeDefinitionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _definitions.CreateAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("definitions/{id:long}")]
    public async Task<ActionResult<ApiResponse<BarcodeDefinitionDto>>> UpdateDefinition(
        long id,
        [FromBody] SaveBarcodeDefinitionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _definitions.UpdateAsync(id, request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("definitions/{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDefinition(long id, CancellationToken cancellationToken = default)
    {
        var result = await _definitions.DeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("resolve")]
    public async Task<ActionResult<ApiResponse<ResolvedBarcodeDto>>> Resolve(
        [FromBody] ResolveBarcodeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var resolved = await _resolution.ResolveAsync(request, cancellationToken);
        var message = resolved.ReasonCode switch
        {
            BarcodeMatchReasonCode.ParsedByDefinition => _localizationService.GetLocalizedString("BarcodeResolvedSuccessfully"),
            BarcodeMatchReasonCode.ResolvedByMirrorLookup => _localizationService.GetLocalizedString("BarcodeResolvedByMirrorLookup"),
            BarcodeMatchReasonCode.InvalidBarcodeFormat => _localizationService.GetLocalizedString("BarcodeInvalidFormat"),
            BarcodeMatchReasonCode.MissingRequiredSegment => _localizationService.GetLocalizedString("BarcodeMissingRequiredSegment"),
            BarcodeMatchReasonCode.DefinitionNotFound => _localizationService.GetLocalizedString("BarcodeDefinitionNotFound"),
            BarcodeMatchReasonCode.AmbiguousMatch => BuildAmbiguousMatchMessage(resolved),
            BarcodeMatchReasonCode.NoMatch => _localizationService.GetLocalizedString("BarcodeNoMatch"),
            _ => _localizationService.GetLocalizedString("BarcodeCouldNotBeResolved")
        };

        var statusCode = resolved.ReasonCode switch
        {
            BarcodeMatchReasonCode.InvalidBarcodeFormat => 400,
            BarcodeMatchReasonCode.MissingRequiredSegment => 400,
            BarcodeMatchReasonCode.DefinitionNotFound => 404,
            BarcodeMatchReasonCode.NoMatch => 404,
            BarcodeMatchReasonCode.AmbiguousMatch => 409,
            _ => 200
        };

        if (statusCode != 200)
        {
            return StatusCode(statusCode, new ApiResponse<ResolvedBarcodeDto>
            {
                Success = false,
                Message = message,
                ExceptionMessage = message,
                ErrorCode = resolved.ReasonCode.ToString(),
                Details = resolved,
                Data = resolved,
                StatusCode = statusCode,
                ClassName = nameof(ApiResponse<ResolvedBarcodeDto>)
            });
        }

        return Ok(ApiResponse<ResolvedBarcodeDto>.SuccessResult(resolved, message));
    }

    private string BuildAmbiguousMatchMessage(ResolvedBarcodeDto resolved)
    {
        var baseMessage = _localizationService.GetLocalizedString("BarcodeAmbiguousMatch");
        if (resolved.Candidates == null || resolved.Candidates.Count == 0)
        {
            return baseMessage;
        }

        var preview = string.Join(", ", resolved.Candidates
            .Take(3)
            .Select(candidate =>
            {
                var stock = string.IsNullOrWhiteSpace(candidate.StockCode) ? "?" : candidate.StockCode;
                var name = string.IsNullOrWhiteSpace(candidate.StockName) ? string.Empty : $" - {candidate.StockName}";
                var yap = string.IsNullOrWhiteSpace(candidate.YapKod) ? string.Empty : $" ({candidate.YapKod})";
                return $"{stock}{name}{yap}";
            }));

        return string.IsNullOrWhiteSpace(preview)
            ? baseMessage
            : $"{baseMessage}: {preview}";
    }
}

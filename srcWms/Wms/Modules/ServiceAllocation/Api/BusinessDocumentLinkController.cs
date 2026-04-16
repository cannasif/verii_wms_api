using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Application.ServiceAllocation.Services;

namespace Wms.WebApi.Controllers.ServiceAllocation;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BusinessDocumentLinkController : ControllerBase
{
    private readonly IBusinessDocumentLinkService _service;

    public BusinessDocumentLinkController(IBusinessDocumentLinkService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-entity")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>>> GetByBusinessEntity(
        [FromQuery] long businessEntityId,
        [FromQuery] Wms.Domain.Entities.ServiceAllocation.Enums.BusinessEntityType businessEntityType,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByBusinessEntityAsync(businessEntityId, businessEntityType, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<BusinessDocumentLinkDto>>>> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<BusinessDocumentLinkDto>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BusinessDocumentLinkDto>>> Create([FromBody] CreateBusinessDocumentLinkDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<BusinessDocumentLinkDto>>> Update(long id, [FromBody] UpdateBusinessDocumentLinkDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<BusinessDocumentLinkDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}

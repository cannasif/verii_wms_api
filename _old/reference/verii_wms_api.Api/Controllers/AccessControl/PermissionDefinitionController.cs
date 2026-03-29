using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/permission-definitions")]
    [Authorize]
    public class PermissionDefinitionController : ControllerBase
    {
        private readonly IPermissionDefinitionService _permissionDefinitionService;

        public PermissionDefinitionController(IPermissionDefinitionService permissionDefinitionService)
        {
            _permissionDefinitionService = permissionDefinitionService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<PermissionDefinitionDto>>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.GetAllAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PermissionDefinitionDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.GetAllAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<PermissionDefinitionDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PermissionDefinitionDto>>> Create([FromBody] CreatePermissionDefinitionDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.CreateAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<PermissionDefinitionDto>>> Update(long id, [FromBody] UpdatePermissionDefinitionDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.UpdateAsync(id, dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("sync")]
        public async Task<ActionResult<ApiResponse<PermissionDefinitionSyncResultDto>>> Sync([FromBody] SyncPermissionDefinitionsDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.SyncAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _permissionDefinitionService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

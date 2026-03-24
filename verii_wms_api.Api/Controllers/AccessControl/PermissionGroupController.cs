using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/permission-groups")]
    [Authorize]
    public class PermissionGroupController : ControllerBase
    {
        private readonly IPermissionGroupService _permissionGroupService;

        public PermissionGroupController(IPermissionGroupService permissionGroupService)
        {
            _permissionGroupService = permissionGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResponse<PermissionGroupDto>>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.GetAllAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<PermissionGroupDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.GetAllAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<PermissionGroupDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PermissionGroupDto>>> Create([FromBody] CreatePermissionGroupDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.CreateAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<PermissionGroupDto>>> Update(long id, [FromBody] UpdatePermissionGroupDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.UpdateAsync(id, dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}/permissions")]
        public async Task<ActionResult<ApiResponse<PermissionGroupDto>>> SetPermissions(long id, [FromBody] SetPermissionGroupPermissionsDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.SetPermissionsAsync(id, dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _permissionGroupService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

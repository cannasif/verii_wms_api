using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/user-permission-groups")]
    [Authorize]
    public class UserPermissionGroupController : ControllerBase
    {
        private readonly IUserPermissionGroupService _userPermissionGroupService;

        public UserPermissionGroupController(IUserPermissionGroupService userPermissionGroupService)
        {
            _userPermissionGroupService = userPermissionGroupService;
        }

        [HttpGet("{userId:long}")]
        public async Task<ActionResult<ApiResponse<UserPermissionGroupDto>>> GetByUserId(long userId, CancellationToken cancellationToken = default)
        {
            var result = await _userPermissionGroupService.GetByUserIdAsync(userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{userId:long}")]
        public async Task<ActionResult<ApiResponse<UserPermissionGroupDto>>> SetUserGroups(long userId, [FromBody] SetUserPermissionGroupsDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _userPermissionGroupService.SetUserGroupsAsync(userId, dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

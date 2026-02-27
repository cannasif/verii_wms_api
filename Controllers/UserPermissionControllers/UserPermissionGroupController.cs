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
        public async Task<ActionResult<ApiResponse<UserPermissionGroupDto>>> GetByUserId(long userId)
        {
            var result = await _userPermissionGroupService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{userId:long}")]
        public async Task<ActionResult<ApiResponse<UserPermissionGroupDto>>> SetUserGroups(long userId, [FromBody] SetUserPermissionGroupsDto dto)
        {
            var result = await _userPermissionGroupService.SetUserGroupsAsync(userId, dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}

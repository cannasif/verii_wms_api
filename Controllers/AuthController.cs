using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPermissionAccessService _permissionAccessService;

        public AuthController(IAuthService authService, IPermissionAccessService permissionAccessService)
        {
            _authService = authService;
            _permissionAccessService = permissionAccessService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("admin-login")]
        public async Task<ActionResult<ApiResponse<string>>> AdminLogin()
        {
            var loginDto = new LoginRequest
            {
                Email = "admin@v3rii.com",
                Password = "Veriipass123!"
            };
            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("me/permissions")]
        public async Task<ActionResult<ApiResponse<MyPermissionsDto>>> GetMyPermissions()
        {
            var result = await _permissionAccessService.GetMyPermissionsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
        {
            var result = await _authService.GetAllUsersAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("users/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetActiveUsers()
        {
            var result = await _authService.GetActiveUsersAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(long id)
        {
            var result = await _authService.GetUserByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterUserAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.RequestPasswordResetAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<ActionResult<ApiResponse<string>>> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.RequestPasswordResetAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return StatusCode(401, ApiResponse<string>.ErrorResult("Unauthorized", "Unauthorized", 401));
            }
            var userId = long.Parse(userIdClaim);
            var result = await _authService.ChangePasswordAsync(userId, request);
            return StatusCode(result.StatusCode, result);
        }

    }
}

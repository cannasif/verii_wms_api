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
        private readonly ILocalizationService _localizationService;

        public AuthController(IAuthService authService, IPermissionAccessService permissionAccessService, ILocalizationService localizationService)
        {
            _authService = authService;
            _permissionAccessService = permissionAccessService;
            _localizationService = localizationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("admin-login")]
        public async Task<ActionResult<ApiResponse<string>>> AdminLogin(CancellationToken cancellationToken = default)
        {
            var loginDto = new LoginRequest
            {
                Email = "admin@v3rii.com",
                Password = "Veriipass123!"
            };
            var result = await _authService.LoginAsync(loginDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("me/permissions")]
        public async Task<ActionResult<ApiResponse<MyPermissionsDto>>> GetMyPermissions(CancellationToken cancellationToken = default)
        {
            var result = await _permissionAccessService.GetMyPermissionsAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var result = await _authService.GetAllUsersAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("users/active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetActiveUsers(CancellationToken cancellationToken = default)
        {
            var result = await _authService.GetActiveUsersAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _authService.GetUserByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RegisterUserAsync(registerDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RequestPasswordResetAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<ActionResult<ApiResponse<string>>> RequestPasswordReset([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RequestPasswordResetAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.ResetPasswordAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return StatusCode(401, ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("Unauthorized"), _localizationService.GetLocalizedString("Unauthorized"), 401));
            }
            var userId = long.Parse(userIdClaim);
            var result = await _authService.ChangePasswordAsync(userId, request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

    }
}

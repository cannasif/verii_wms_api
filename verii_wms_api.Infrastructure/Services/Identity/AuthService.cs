using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;
using Hangfire;
using WMS_WEBAPI.Security;

namespace WMS_WEBAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILocalizationService _localizationService;
        private readonly WmsDbContext _context;
        private readonly IHubContext<WMS_WEBAPI.Hubs.AuthHub> _hubContext;
        private readonly IResetPasswordEmailJob _resetPasswordEmailJob;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            ILocalizationService localizationService,
            WmsDbContext context,
            IHubContext<WMS_WEBAPI.Hubs.AuthHub> hubContext,
            IResetPasswordEmailJob resetPasswordEmailJob)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _localizationService = localizationService;
            _context = context;
            _hubContext = hubContext;
            _resetPasswordEmailJob = resetPasswordEmailJob;
        }

        public async Task<ApiResponse<UserDto>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var query = _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation);
                var user = await query.FirstOrDefaultAsync(u => u.Username == username);
                
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<UserDto>.ErrorResult(nf, nf, 404);
                }

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation)
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();
                
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<UserDto>.ErrorResult(nf, nf, 404);
                }

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUserResponse = await GetUserByUsernameAsync(registerDto.Username);
                if (existingUserResponse.Success)
                {
                    var msg = _localizationService.GetLocalizedString("AuthUserAlreadyExists");
                    return ApiResponse<UserDto>.ErrorResult(msg, msg, 400);
                }

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var dto = MapToUserDto(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _localizationService.GetLocalizedString("AuthUserRegisteredSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(_localizationService.GetLocalizedString("AuthRegistrationFailed"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginRequest request)
        {
            try
            {
                var loginDto = new LoginDto
                {
                    Username = request.Email,
                    Password = request.Password
                };
                // Email veya username ile kullanıcı arama
                var user = await _unitOfWork.Users.AsQueryable()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.Username == loginDto.Username || u.Email == loginDto.Username)
                    .FirstOrDefaultAsync();
                
                if (user == null)
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    return ApiResponse<string>.ErrorResult(msg, msg, 401);
                }
                
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    return ApiResponse<string>.ErrorResult(msg, msg, 401);
                }

                var (permissions, isSystemAdmin) = await GetPermissionClaimsAsync(user.Id, user.RoleId);
                var tokenResponse = _jwtService.GenerateToken(user, permissions, isSystemAdmin);
                if (!tokenResponse.Success)
                {
                    return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("Error.User.LoginFailed"), tokenResponse.Message ?? string.Empty, 500);
                }
                var token = tokenResponse.Data!;

                var activeSession = _context.Set<UserSession>().FirstOrDefault(s => s.UserId == user.Id && s.RevokedAt == null);
                if (activeSession != null)
                {
                    activeSession.RevokedAt = DateTimeProvider.Now;
                    _context.SaveChanges();
                    await WMS_WEBAPI.Hubs.AuthHub.ForceLogoutUser(_hubContext, user.Id.ToString());
                }

                var session = new UserSession
                {
                    UserId = user.Id,
                    SessionId = Guid.NewGuid(),
                    CreatedAt = DateTimeProvider.Now,
                    Token = ComputeSha256Hash(token),
                    IsDeleted = false,
                    CreatedDate = DateTimeProvider.Now
                };
                _context.Set<UserSession>().Add(session);
                _context.SaveChanges();
                
                return ApiResponse<string>.SuccessResult(token, _localizationService.GetLocalizedString("Success.User.LoginSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("Error.User.LoginFailed"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.AsQueryable().Include(u => u.RoleNavigation).ToListAsync();
                var dtos = users.Select(MapToUserDto).ToList();
                return ApiResponse<IEnumerable<UserDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("DataRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.AsQueryable()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.IsActive == true)
                    .ToListAsync();
                var dtos = users.Select(MapToUserDto).ToList();
                return ApiResponse<IEnumerable<UserDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ActiveUsersRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<string>> RequestPasswordResetAsync(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable().FirstOrDefaultAsync(u => u.Email == request.Email);
                var token = Guid.NewGuid().ToString("N");
                var tokenHash = ComputeSha256Hash(token);
                var expiresAt = DateTime.UtcNow.AddMinutes(30);

                if (user != null)
                {
                    var reset = new PasswordResetRequest
                    {
                        UserId = user.Id,
                        TokenHash = tokenHash,
                        ExpiresAt = expiresAt,
                        CreatedDate = DateTimeProvider.Now,
                        IsDeleted = false
                    };
                    _context.Set<PasswordResetRequest>().Add(reset);
                    await _context.SaveChangesAsync();
                    
                    var fullName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
                    if (string.IsNullOrWhiteSpace(fullName))
                    {
                        fullName = user.Username;
                    }
                    BackgroundJob.Enqueue<IResetPasswordEmailJob>(job =>
                        job.SendPasswordResetEmailAsync(user.Email, fullName, token));
                }

                var msg = _localizationService.GetLocalizedString("OperationSuccessful");
                return ApiResponse<string>.SuccessResult(string.Empty, msg);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var tokenHash = ComputeSha256Hash(request.Token);
                var now = DateTime.UtcNow;

                var reset = await _context.Set<PasswordResetRequest>()
                    .Include(r => r.User)
                    .Where(r => r.TokenHash == tokenHash && r.UsedAt == null && r.ExpiresAt > now && !r.IsDeleted)
                    .FirstOrDefaultAsync();

                if (reset == null || reset.User == null)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ValidationError"), _localizationService.GetLocalizedString("ValidationError"), 400);
                }

                reset.UsedAt = now;
                reset.UpdatedDate = now;

                reset.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                reset.User.UpdatedDate = now;

                await _context.SaveChangesAsync();

                await InvalidateUserSessionsAsync(reset.User.Id);

                var displayName = string.Join(" ", new[] { reset.User.FirstName, reset.User.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = reset.User.Username;
                }
                BackgroundJob.Enqueue<IResetPasswordEmailJob>(job =>
                    job.SendPasswordResetCompletedEmailAsync(reset.User.Email, displayName));

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(long userId, ChangePasswordRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.AsQueryable()
                    .Include(u => u.RoleNavigation)
                    .Where(u => u.Id == userId)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    var nf = _localizationService.GetLocalizedString("AuthUserNotFound");
                    return ApiResponse<string>.ErrorResult(nf, nf, 404);
                }

                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                {
                    var msg = _localizationService.GetLocalizedString("Error.User.CurrentPasswordIncorrect");
                    if (msg == "Error.User.CurrentPasswordIncorrect")
                    {
                        msg = _localizationService.GetLocalizedString("Error.User.InvalidCredentials");
                    }
                    return ApiResponse<string>.ErrorResult(msg, msg, 400);
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.UpdatedDate = DateTimeProvider.Now;
                var affectedRows = await _unitOfWork.SaveChangesAsync();
                if (affectedRows == 0 || !BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
                {
                    var saveMsg = _localizationService.GetLocalizedString("AuthErrorOccurred");
                    return ApiResponse<string>.ErrorResult(saveMsg, saveMsg, 500);
                }

                await InvalidateUserSessionsAsync(user.Id);

                var (permissions, isSystemAdmin) = await GetPermissionClaimsAsync(user.Id, user.RoleId);
                var tokenResponse = _jwtService.GenerateToken(user, permissions, isSystemAdmin);
                if (!tokenResponse.Success || string.IsNullOrWhiteSpace(tokenResponse.Data))
                {
                    return ApiResponse<string>.ErrorResult(
                        _localizationService.GetLocalizedString("Error.User.LoginFailed"),
                        tokenResponse.ExceptionMessage,
                        tokenResponse.StatusCode == 0 ? 500 : tokenResponse.StatusCode);
                }

                var newToken = tokenResponse.Data!;
                var session = new UserSession
                {
                    UserId = user.Id,
                    SessionId = Guid.NewGuid(),
                    CreatedAt = DateTimeProvider.Now,
                    Token = ComputeSha256Hash(newToken),
                    IsDeleted = false,
                    CreatedDate = DateTimeProvider.Now
                };
                _context.Set<UserSession>().Add(session);
                await _context.SaveChangesAsync();

                var displayName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = user.Username;
                }
                BackgroundJob.Enqueue<IResetPasswordEmailJob>(job =>
                    job.SendPasswordChangedEmailAsync(user.Email, displayName));

                return ApiResponse<string>.SuccessResult(newToken, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("AuthErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private async Task InvalidateUserSessionsAsync(long userId)
        {
            var sessions = await _context.Set<UserSession>()
                .Where(s => s.UserId == userId && s.RevokedAt == null)
                .ToListAsync();

            if (sessions.Count > 0)
            {
                var now = DateTime.UtcNow;
                foreach (var s in sessions)
                {
                    s.RevokedAt = now;
                    s.UpdatedDate = now;
                }
                await _context.SaveChangesAsync();
                await WMS_WEBAPI.Hubs.AuthHub.ForceLogoutUser(_hubContext, userId.ToString());
            }
        }

        private async Task<(IReadOnlyCollection<string> Permissions, bool IsSystemAdmin)> GetPermissionClaimsAsync(long userId, long roleId)
        {
            var roleTitle = await _unitOfWork.UserAuthorities.AsQueryable()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Id == roleId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();

            var userGroupLinks = await _unitOfWork.UserPermissionGroups.AsQueryable()
                .AsNoTracking()
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Include(x => x.PermissionGroup)
                    .ThenInclude(x => x.GroupPermissions.Where(gp => !gp.IsDeleted))
                        .ThenInclude(x => x.PermissionDefinition)
                .ToListAsync();

            var isSystemAdmin = userGroupLinks.Any(x => x.PermissionGroup.IsSystemAdmin);
            if (!isSystemAdmin &&
                userGroupLinks.Count == 0 &&
                !string.IsNullOrWhiteSpace(roleTitle) &&
                roleTitle.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                isSystemAdmin = true;
            }

            var permissions = userGroupLinks
                .SelectMany(x => x.PermissionGroup.GroupPermissions)
                .Where(x => !x.IsDeleted && x.PermissionDefinition != null && !x.PermissionDefinition.IsDeleted && x.PermissionDefinition.IsActive)
                .Select(x => x.PermissionDefinition.Code)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            return (permissions, isSystemAdmin);
        }

        static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.RoleNavigation?.Title ?? "User",
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                FullName = user.FullName
            };
        }
    }
}

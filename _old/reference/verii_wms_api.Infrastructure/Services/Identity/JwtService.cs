using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Security;

namespace WMS_WEBAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILocalizationService _localizationService;

        public JwtService(IConfiguration configuration, ILocalizationService localizationService)
        {
            _configuration = configuration;
            _localizationService = localizationService;
        }

        public ApiResponse<string> GenerateToken(User user, IReadOnlyCollection<string>? permissions = null, bool isSystemAdmin = false)
        {
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim("firstName", user.FirstName ?? ""),
    new Claim("lastName", user.LastName ?? ""),
    new Claim(ClaimTypes.Role, user.RoleNavigation?.Title ?? "User")
};

if (isSystemAdmin)
{
    claims.Add(new Claim(ClaimConstants.SystemAdmin, "true"));
}

if (permissions != null)
{
    claims.AddRange(
        permissions
            .Where(permission => !string.IsNullOrWhiteSpace(permission))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(permission => new Claim(ClaimConstants.Permission, permission)));
}

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
    issuer: _configuration["Jwt:Issuer"],
    audience: _configuration["Jwt:Audience"],
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60")),
    signingCredentials: credentials
);

var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
return ApiResponse<string>.SuccessResult(tokenString, _localizationService.GetLocalizedString("TokenGeneratedSuccessfully"));
        }
    }
}

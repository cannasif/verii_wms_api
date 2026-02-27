using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.DTOs;

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

        public ApiResponse<string> GenerateToken(User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("firstName", user.FirstName ?? ""),
                    new Claim("lastName", user.LastName ?? ""),
                    new Claim(ClaimTypes.Role, user.RoleNavigation?.Title ?? "User")
                };

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
            catch (Exception ex)
            {
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("TokenGenerationError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}

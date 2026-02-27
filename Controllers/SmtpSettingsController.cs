using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SmtpSettingsController : ControllerBase
    {
        private readonly ISmtpSettingsService _smtpSettingsService;
        private readonly ILocalizationService _localizationService;

        public SmtpSettingsController(ISmtpSettingsService smtpSettingsService, ILocalizationService localizationService)
        {
            _smtpSettingsService = smtpSettingsService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SmtpSettingsDto>>> Get()
        {
            var res = await _smtpSettingsService.GetAsync();
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<SmtpSettingsDto>>> Update([FromBody] UpdateSmtpSettingsDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                var unauth = ApiResponse<SmtpSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Unauthorized"),
                    _localizationService.GetLocalizedString("Unauthorized"),
                    StatusCodes.Status401Unauthorized);

                return StatusCode(unauth.StatusCode, unauth);
            }

            var res = await _smtpSettingsService.UpdateAsync(dto, userId);
            return StatusCode(res.StatusCode, res);
        }
    }
}

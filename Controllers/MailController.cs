using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;
        private readonly ISmtpSettingsService _smtpSettingsService;

        public MailController(
            IMailService mailService,
            IUnitOfWork unitOfWork,
            ILocalizationService localizationService,
            ISmtpSettingsService smtpSettingsService)
        {
            _mailService = mailService;
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
            _smtpSettingsService = smtpSettingsService;
        }

        [HttpPost("send-test")]
        public async Task<ActionResult<ApiResponse<bool>>> SendTest([FromBody] SendTestMailDto dto)
        {
            try
            {
                SmtpSettingsRuntimeDto smtp;
                try
                {
                    smtp = await _smtpSettingsService.GetRuntimeAsync();
                }
                catch
                {
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            "SMTP settings are missing or cannot be decrypted.",
                            StatusCodes.Status400BadRequest));
                }

                if (string.IsNullOrWhiteSpace(smtp.Host) ||
                    string.IsNullOrWhiteSpace(smtp.Username) ||
                    string.IsNullOrWhiteSpace(smtp.Password) ||
                    string.IsNullOrWhiteSpace(smtp.FromEmail))
                {
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            "SMTP settings are incomplete.",
                            StatusCodes.Status400BadRequest));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                {
                    var unauth = ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("Unauthorized"),
                        _localizationService.GetLocalizedString("Unauthorized"),
                        StatusCodes.Status401Unauthorized);
                    return StatusCode(unauth.StatusCode, unauth);
                }

                var to = dto.To;
                if (string.IsNullOrWhiteSpace(to))
                {
                    var user = await _unitOfWork.Users.AsQueryable()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);

                    if (user == null || string.IsNullOrWhiteSpace(user.Email))
                    {
                        var notFound = ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("UserNotFound"),
                            _localizationService.GetLocalizedString("UserNotFound"),
                            StatusCodes.Status404NotFound);
                        return StatusCode(notFound.StatusCode, notFound);
                    }

                    to = user.Email;
                }

                var subject = "SMTP Test Mail";
                var body = $"SMTP test email sent at {DateTime.UtcNow:O}";

                var ok = await _mailService.SendEmailAsync(to, subject, body, false, null, null, null);
                if (!ok)
                {
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            "Failed to send test mail.",
                            StatusCodes.Status400BadRequest));
                }

                return StatusCode(
                    StatusCodes.Status200OK,
                    ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful")));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("InternalServerError"),
                        ex.Message,
                        StatusCodes.Status500InternalServerError));
            }
        }
    }
}

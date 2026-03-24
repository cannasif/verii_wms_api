using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SmtpSettingsService : ISmtpSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IMemoryCache _cache;
        private readonly IDataProtector _protector;
        private readonly IConfiguration _configuration;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        private const string CacheKey = "smtp_settings_runtime_v1";

        public SmtpSettingsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IMemoryCache cache,
            IConfiguration configuration,
            IDataProtectionProvider dataProtectionProvider,
            IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _cache = cache;
            _configuration = configuration;
            _protector = dataProtectionProvider.CreateProtector("smtp-settings-v1");
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        public void InvalidateCache()
        {
            _cache.Remove(CacheKey);
        }

        public async Task<ApiResponse<SmtpSettingsDto>> GetAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await EnsureSeededSettingsAsync(requestCancellationToken);

                return ApiResponse<SmtpSettingsDto>.SuccessResult(
                    _mapper.Map<SmtpSettingsDto>(entity),
                    _localizationService.GetLocalizedString("DataRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SmtpSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SmtpSettingsDto>> UpdateAsync(UpdateSmtpSettingsDto dto, long userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await _unitOfWork.SmtpSettings.Query()
                    .OrderBy(x => x.Id)
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync(requestCancellationToken);

                if (entity == null)
                {
                    // Soft-deleted record exists: revive and reuse it instead of inserting explicit Id.
                    entity = await _unitOfWork.SmtpSettings.Query(ignoreQueryFilters: true)
                        .OrderBy(x => x.Id)
                        .FirstOrDefaultAsync(requestCancellationToken);

                    if (entity != null)
                    {
                        entity.IsDeleted = false;
                        entity.DeletedDate = null;
                        entity.DeletedBy = null;
                    }
                }

                if (entity == null)
                {
                    entity = new SmtpSetting
                    {
                        IsDeleted = false,
                        CreatedDate = DateTimeProvider.Now,
                        CreatedBy = userId
                    };

                    _mapper.Map(dto, entity);
                    if (!string.IsNullOrWhiteSpace(dto.Password))
                    {
                        entity.PasswordEncrypted = _protector.Protect(dto.Password);
                    }

                    entity.UpdatedDate = DateTimeProvider.Now;
                    entity.UpdatedBy = userId;

                    await _unitOfWork.SmtpSettings.AddAsync(entity, requestCancellationToken);
                    await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                    InvalidateCache();

                    return ApiResponse<SmtpSettingsDto>.SuccessResult(
                        _mapper.Map<SmtpSettingsDto>(entity),
                        _localizationService.GetLocalizedString("OperationSuccessful"));
                }

                _mapper.Map(dto, entity);
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    entity.PasswordEncrypted = _protector.Protect(dto.Password);
                }

                entity.UpdatedDate = DateTimeProvider.Now;
                entity.UpdatedBy = userId;

                _unitOfWork.SmtpSettings.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                InvalidateCache();

                return ApiResponse<SmtpSettingsDto>.SuccessResult(
                    _mapper.Map<SmtpSettingsDto>(entity),
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SmtpSettingsDto>.ErrorResult(
                    _localizationService.GetLocalizedString("InternalServerError"),
                    ex.GetBaseException().Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<SmtpSettingsRuntimeDto> GetRuntimeAsync(CancellationToken cancellationToken = default)
        {
            if (_cache.TryGetValue(CacheKey, out SmtpSettingsRuntimeDto? cached) && cached != null)
            {
                return cached;
            }

            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            var entity = await EnsureSeededSettingsAsync(requestCancellationToken);

            var password = string.IsNullOrWhiteSpace(entity.PasswordEncrypted)
                ? string.Empty
                : _protector.Unprotect(entity.PasswordEncrypted);

            var runtime = new SmtpSettingsRuntimeDto
            {
                Host = entity.Host ?? string.Empty,
                Port = entity.Port,
                EnableSsl = entity.EnableSsl,
                Username = entity.Username ?? string.Empty,
                Password = password,
                FromEmail = entity.FromEmail ?? string.Empty,
                FromName = entity.FromName ?? string.Empty,
                Timeout = entity.Timeout
            };

            _cache.Set(CacheKey, runtime);
            return runtime;
        }

        private async Task<SmtpSetting> EnsureSeededSettingsAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            var entity = await _unitOfWork.SmtpSettings.Query()
                .OrderBy(x => x.Id)
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(requestCancellationToken);

            if (entity != null)
            {
                return entity;
            }

            // If there is a soft-deleted SMTP row, revive and update defaults instead of creating a new identity row.
            entity = await _unitOfWork.SmtpSettings.Query(ignoreQueryFilters: true)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(requestCancellationToken);

            var host = _configuration["Smtp:Host"] ?? "localhost";
            var username = _configuration["Smtp:Username"] ?? string.Empty;
            var password = _configuration["Smtp:Password"] ?? string.Empty;
            var fromEmail = _configuration["Smtp:FromAddress"]
                            ?? _configuration["Smtp:FromEmail"]
                            ?? "no-reply@localhost";
            var fromName = _configuration["Smtp:FromName"] ?? "VERII WMS";

            var port = 587;
            if (int.TryParse(_configuration["Smtp:Port"], out var configuredPort) && configuredPort > 0)
            {
                port = configuredPort;
            }

            var timeout = 30;
            if (int.TryParse(_configuration["Smtp:Timeout"], out var configuredTimeout) && configuredTimeout > 0)
            {
                timeout = configuredTimeout;
            }

            var enableSsl = false;
            if (bool.TryParse(_configuration["Smtp:EnableSsl"], out var configuredEnableSsl))
            {
                enableSsl = configuredEnableSsl;
            }

            if (entity == null)
            {
                entity = new SmtpSetting
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enableSsl,
                    Username = username,
                    PasswordEncrypted = string.IsNullOrWhiteSpace(password) ? string.Empty : _protector.Protect(password),
                    FromEmail = fromEmail,
                    FromName = fromName,
                    Timeout = timeout,
                    IsDeleted = false,
                    CreatedDate = DateTimeProvider.Now
                };
                await _unitOfWork.SmtpSettings.AddAsync(entity, requestCancellationToken);
            }
            else
            {
                entity.Host = host;
                entity.Port = port;
                entity.EnableSsl = enableSsl;
                entity.Username = username;
                entity.PasswordEncrypted = string.IsNullOrWhiteSpace(password) ? string.Empty : _protector.Protect(password);
                entity.FromEmail = fromEmail;
                entity.FromName = fromName;
                entity.Timeout = timeout;
                entity.IsDeleted = false;
                entity.DeletedDate = null;
                entity.DeletedBy = null;
                entity.UpdatedDate = DateTimeProvider.Now;
                _unitOfWork.SmtpSettings.Update(entity);
            }

            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
            InvalidateCache();

            return entity;
        }
    }
}

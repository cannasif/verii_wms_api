using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Repositories;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.Services.Jobs;
using WMS_WEBAPI.UnitOfWork;
using WMS_WEBAPI.Options;

namespace WMS_WEBAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(Program).Assembly,
            typeof(WMS_WEBAPI.Mappings.WmsAutoMapperProfile).Assembly,
            typeof(WMS_WEBAPI.Services.LocalizationService).Assembly);

        return services;
    }

    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var dataProtectionKeyPath =
            configuration["DataProtection:KeyPath"] ??
            Path.Combine(environment.ContentRootPath, "DataProtectionKeys");
        Directory.CreateDirectory(dataProtectionKeyPath);

        services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyPath))
            .SetApplicationName("V3RII_WMS");

        services.AddDbContextPool<WmsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=wms.db";
            options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
        });

        services.AddDbContextPool<ErpDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ErpConnection");
            options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHangfire(hangfireConfiguration => hangfireConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.Configure<HangfireMonitoringOptions>(
            configuration.GetSection(HangfireMonitoringOptions.SectionName));

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
        {
            Attempts = 3,
            DelaysInSeconds = new[] { 60, 300, 900 },
            LogEvents = true,
            OnAttemptsExceeded = AttemptsExceededAction.Fail
        });

        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { "default", "email", "reset-pass-mail", "dead-letter" };
        });

        services.AddScoped<IUnitOfWork, WMS_WEBAPI.UnitOfWork.UnitOfWork>();
        services.AddScoped<IErpUnitOfWork, WMS_WEBAPI.UnitOfWork.ErpUnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRequestCancellationAccessor, HttpRequestCancellationAccessor>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserAuthorityService, UserAuthorityService>();
        services.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IPermissionDefinitionService, PermissionDefinitionService>();
        services.AddScoped<IPermissionGroupService, PermissionGroupService>();
        services.AddScoped<IUserPermissionGroupService, UserPermissionGroupService>();
        services.AddScoped<IPermissionAccessService, PermissionAccessService>();
        services.AddScoped<ICustomerMirrorService, CustomerMirrorService>();
        services.AddScoped<IStockMirrorService, StockMirrorService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<IErpService, ErpService>();
        services.AddScoped<IPlatformPageGroupService, PlatformPageGroupService>();
        services.AddScoped<IPlatformUserGroupMatchService, PlatformUserGroupMatchService>();
        services.AddScoped<IMobilePageGroupService, MobilePageGroupService>();
        services.AddScoped<IMobileUserGroupMatchService, MobileUserGroupMatchService>();
        services.AddScoped<IGrHeaderService, GrHeaderService>();
        services.AddScoped<IGrHeaderService, GrHeaderService>();
        services.AddScoped<IGrLineService, GrLineService>();
        services.AddScoped<IGrImportDocumentService, GrImportDocumentService>();
        services.AddScoped<IGrImportLineService, GrImportLineService>();
        services.AddScoped<IGrRouteService, GrRouteService>();
        services.AddScoped<IGrLineSerialService, GrLineSerialService>();
        services.AddScoped<IGrTerminalLineService, GrTerminalLineService>();
        services.AddScoped<IWtFunctionService, WtFunctionService>();
        services.AddScoped<IWtHeaderService, WtHeaderService>();
        services.AddScoped<IWtLineService, WtLineService>();
        services.AddScoped<IWtImportLineService, WtImportLineService>();
        services.AddScoped<IWtLineSerialService, WtLineSerialService>();
        services.AddScoped<IWtRouteService, WtRouteService>();
        services.AddScoped<IWtTerminalLineService, WtTerminalLineService>();
        services.AddScoped<IPtFunctionService, PtFunctionService>();
        services.AddScoped<IPtHeaderService, PtHeaderService>();
        services.AddScoped<IPtLineService, PtLineService>();
        services.AddScoped<IPtImportLineService, PtImportLineService>();
        services.AddScoped<IPtRouteService, PtRouteService>();
        services.AddScoped<IPtTerminalLineService, PtTerminalLineService>();
        services.AddScoped<IPtLineSerialService, PtLineSerialService>();
        services.AddScoped<IPrFunctionService, PrFunctionService>();
        services.AddScoped<IPrHeaderService, PrHeaderService>();
        services.AddScoped<IPrLineService, PrLineService>();
        services.AddScoped<IPrImportLineService, PrImportLineService>();
        services.AddScoped<IPrRouteService, PrRouteService>();
        services.AddScoped<IPrTerminalLineService, PrTerminalLineService>();
        services.AddScoped<IPrLineSerialService, PrLineSerialService>();
        services.AddScoped<IPrHeaderSerialService, PrHeaderSerialService>();
        services.AddScoped<ISitHeaderService, SitHeaderService>();
        services.AddScoped<ISitLineService, SitLineService>();
        services.AddScoped<ISitImportLineService, SitImportLineService>();
        services.AddScoped<ISitRouteService, SitRouteService>();
        services.AddScoped<ISitTerminalLineService, SitTerminalLineService>();
        services.AddScoped<ISitLineSerialService, SitLineSerialService>();
        services.AddScoped<ISrtHeaderService, SrtHeaderService>();
        services.AddScoped<ISrtLineService, SrtLineService>();
        services.AddScoped<ISrtImportLineService, SrtImportLineService>();
        services.AddScoped<ISrtRouteService, SrtRouteService>();
        services.AddScoped<ISrtTerminalLineService, SrtTerminalLineService>();
        services.AddScoped<ISrtFunctionService, SrtFunctionService>();
        services.AddScoped<ISrtLineSerialService, SrtLineSerialService>();
        services.AddScoped<IWoFunctionService, WoFunctionService>();
        services.AddScoped<IWoHeaderService, WoHeaderService>();
        services.AddScoped<IWoLineService, WoLineService>();
        services.AddScoped<IWoImportLineService, WoImportLineService>();
        services.AddScoped<IWoRouteService, WoRouteService>();
        services.AddScoped<IWoTerminalLineService, WoTerminalLineService>();
        services.AddScoped<IWoLineSerialService, WoLineSerialService>();
        services.AddScoped<IWiFunctionService, WiFunctionService>();
        services.AddScoped<IWiHeaderService, WiHeaderService>();
        services.AddScoped<IWiLineService, WiLineService>();
        services.AddScoped<IWiImportLineService, WiImportLineService>();
        services.AddScoped<IWiRouteService, WiRouteService>();
        services.AddScoped<IWiTerminalLineService, WiTerminalLineService>();
        services.AddScoped<IWiLineSerialService, WiLineSerialService>();
        services.AddScoped<IShHeaderService, ShHeaderService>();
        services.AddScoped<IShFunctionService, ShFunctionService>();
        services.AddScoped<IShLineService, ShLineService>();
        services.AddScoped<IShImportLineService, ShImportLineService>();
        services.AddScoped<IShRouteService, ShRouteService>();
        services.AddScoped<IShTerminalLineService, ShTerminalLineService>();
        services.AddScoped<IShLineSerialService, ShLineSerialService>();
        services.AddScoped<IGrParameterService, GrParameterService>();
        services.AddScoped<IWtParameterService, WtParameterService>();
        services.AddScoped<IWoParameterService, WoParameterService>();
        services.AddScoped<IWiParameterService, WiParameterService>();
        services.AddScoped<IShParameterService, ShParameterService>();
        services.AddScoped<ISrtParameterService, SrtParameterService>();
        services.AddScoped<ISitParameterService, SitParameterService>();
        services.AddScoped<IPtParameterService, PtParameterService>();
        services.AddScoped<IPrParameterService, PrParameterService>();
        services.AddScoped<IIcParameterService, IcParameterService>();
        services.AddScoped<IPParameterService, PParameterService>();
        services.AddScoped<IGoodReciptFunctionsService, GoodReciptFunctionsService>();
        services.AddScoped<BackgroundJobService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserDetailService, UserDetailService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IResetPasswordEmailJob, ResetPasswordEmailJob>();
        services.AddScoped<IStockSyncJob, StockSyncJob>();
        services.AddScoped<ICustomerSyncJob, CustomerSyncJob>();
        services.AddScoped<IHangfireDeadLetterJob, HangfireDeadLetterJob>();
        services.AddScoped<IPHeaderService, PHeaderService>();
        services.AddScoped<IPPackageService, PPackageService>();
        services.AddScoped<IPLineService, PLineService>();

        return services;
    }
}

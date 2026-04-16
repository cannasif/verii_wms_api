using AutoMapper;
using System.Reflection;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using Wms.Application.AccessControl.Services;
using Wms.Application.Common;
using Wms.Application.Communications.Services;
using Wms.Application.Definitions.Services.Parameters;
using Wms.Application.GoodsReceipt.Services;
using Wms.Application.Identity.Services;
using Wms.Application.Package.Services;
using Wms.Application.Customer.Services;
using Wms.Application.System.Services;
using Wms.Application.InventoryCount.Services;
using Wms.Application.Stock.Services;
using Wms.Application.ServiceAllocation.Services;
using Wms.Application.WarehouseTransfer.Mappings;
using Wms.Application.WarehouseTransfer.Services;
using Wms.Application.Warehouse.Services;
using Wms.Application.WarehouseOutbound.Services;
using Wms.Application.WarehouseInbound.Services;
using Wms.Application.Shipping.Services;
using Wms.Application.YapKod.Services;
using Wms.Application.Production.Services;
using Wms.Application.ProductionTransfer.Services;
using Wms.Application.SubcontractingIssueTransfer.Services;
using Wms.Application.SubcontractingReceiptTransfer.Services;
using Wms.Infrastructure.Persistence.Context;
using Wms.Infrastructure.Persistence.Repositories;
using Wms.Infrastructure.Persistence.UnitOfWork;
using Wms.Infrastructure.Options;
using Wms.Infrastructure.Services.Communications;
using Wms.Infrastructure.Services.Common;
using Wms.Infrastructure.Services.Customer;
using Wms.Infrastructure.Services.Erp;
using Wms.Infrastructure.Services.Files;
using Wms.Infrastructure.Services.GoodsReceipt;
using Wms.Infrastructure.Services.Integrations;
using Wms.Infrastructure.Services.WarehouseTransfer;
using Wms.Infrastructure.Services.WarehouseOutbound;
using Wms.Infrastructure.Services.WarehouseInbound;
using Wms.Infrastructure.Services.Shipping;
using Wms.Infrastructure.Services.Stock;
using Wms.Infrastructure.Services.Warehouse;
using Wms.Infrastructure.Services.YapKod;
using Wms.Infrastructure.Services.Production;
using Wms.Infrastructure.Services.ProductionTransfer;
using Wms.Infrastructure.Services.SubcontractingIssueTransfer;
using Wms.Infrastructure.Services.SubcontractingReceiptTransfer;
using Wms.Infrastructure.Services.Identity;
using Wms.Infrastructure.Services.Localization;
using Wms.Infrastructure.Services.Security;
using Wms.WebApi.Options;
using Wms.WebApi.Realtime;

namespace Wms.WebApi.Extensions;

/// <summary>
/// Yeni pragmatik WebApi katmanının DI kayıt başlangıç noktasıdır.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPragmaticWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.Configure<HangfireMonitoringOptions>(configuration.GetSection("HangfireMonitoring"));
        services.Configure<PragmaticCorsOptions>(configuration.GetSection("Cors"));
        services.Configure<BarcodeDefinitionsOptions>(configuration.GetSection("BarcodeDefinitions"));

        var corsOptions = configuration.GetSection("Cors").Get<PragmaticCorsOptions>() ?? new PragmaticCorsOptions();
        services.AddCors(options =>
        {
            options.AddPolicy("PragmaticCors", policy =>
            {
                if (corsOptions.AllowedOrigins.Count > 0)
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins.ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
                else
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? configuration.GetConnectionString("Default")
            ?? "Server=(localdb)\\mssqllocaldb;Database=Wms;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<WmsDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true,
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { "default", "dead-letter" };
        });

        var jwtSecret = configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "WMS_API";
        var jwtAudience = configuration["Jwt:Audience"] ?? "WMS_Client";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrWhiteSpace(accessToken)
                            && (path.StartsWithSegments("/notificationHub") || path.StartsWithSegments("/hubs/notifications")))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAutoMapper(_ => { }, typeof(Wms.Application.Common.AssemblyMarker).Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(Wms.Application.Common.AssemblyMarker).Assembly);
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddDataProtection();
        services.AddSignalR();

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IErpUnitOfWork, ErpUnitOfWorkAdapter>();
        foreach (var resourceType in typeof(AssemblyMarker).Assembly
                     .GetTypes()
                     .Where(type => typeof(ILocalizationResource).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false }))
        {
            services.AddSingleton(typeof(ILocalizationResource), resourceType);
        }
        services.AddSingleton<LocalizationRegistry>();
        services.AddScoped<ILocalizationService, PragmaticLocalizationService>();
        services.AddScoped<ICurrentUserAccessor, HttpCurrentUserAccessor>();
        services.AddScoped<ICurrentUserService, CurrentUserServiceAdapter>();
        services.AddScoped<IRequestCancellationAccessor, RequestCancellationAccessor>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IEntityReferenceResolver, EntityReferenceResolver>();
        services.AddScoped<IDocumentReferenceReadEnricher, DocumentReferenceReadEnricher>();
        services.AddScoped<IAssignedBarcodeMatchingService, AssignedBarcodeMatchingService>();
        services.AddScoped<IBarcodeDefinitionService, BarcodeDefinitionService>();
        services.AddSingleton<IBarcodeParser, BarcodeParser>();
        services.AddScoped<IBarcodeResolutionService, BarcodeResolutionService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IResetPasswordEmailJob, ResetPasswordEmailJob>();
        services.AddScoped<IHangfireDeadLetterJob, HangfireDeadLetterJob>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IJobFailureLogWriter, JobFailureLogWriter>();
        services.AddScoped<IPermissionDefinitionService, PermissionDefinitionService>();
        services.AddScoped<IPermissionGroupService, PermissionGroupService>();
        services.AddScoped<IUserPermissionGroupService, UserPermissionGroupService>();
        services.AddScoped<IPermissionAccessService, PermissionAccessService>();
        services.AddScoped<IErpService, PragmaticErpService>();
        services.AddScoped<IJobService, JobService>();
        services.AddSingleton<IHangfireManualSyncService, HangfireManualSyncService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserAuthorityService, UserAuthorityService>();
        services.AddScoped<IUserDetailService, UserDetailService>();
        services.AddScoped<IPrHeaderSerialService, PrHeaderSerialService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerErpReadService, CustomerErpReadService>();
        services.AddScoped<ICustomerSyncJob, CustomerSyncJob>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IServiceCaseService, ServiceCaseService>();
        services.AddScoped<IServiceCaseLineService, ServiceCaseLineService>();
        services.AddScoped<IAllocationEngine, AllocationEngine>();
        services.AddScoped<IOperationAllocationOrchestrator, OperationAllocationOrchestrator>();
        services.AddScoped<IOrderAllocationLineService, OrderAllocationLineService>();
        services.AddScoped<IBusinessDocumentLinkService, BusinessDocumentLinkService>();
        services.AddScoped<IStockErpReadService, StockErpReadService>();
        services.AddScoped<IStockSyncJob, StockSyncJob>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IWarehouseErpReadService, WarehouseErpReadService>();
        services.AddScoped<IWarehouseSyncJob, WarehouseSyncJob>();
        services.AddScoped<IYapKodService, YapKodService>();
        services.AddScoped<IYapKodErpReadService, YapKodErpReadService>();
        services.AddScoped<IYapKodSyncJob, YapKodSyncJob>();

        services.AddScoped<IGrParameterService, GrParameterService>();
        services.AddScoped<IIcParameterService, IcParameterService>();
        services.AddScoped<IPParameterService, PParameterService>();
        services.AddScoped<IPrParameterService, PrParameterService>();
        services.AddScoped<IPtParameterService, PtParameterService>();
        services.AddScoped<IShParameterService, ShParameterService>();
        services.AddScoped<ISitParameterService, SitParameterService>();
        services.AddScoped<ISrtParameterService, SrtParameterService>();
        services.AddScoped<IIcHeaderService, IcHeaderService>();
        services.AddScoped<IIcScopeService, IcScopeService>();
        services.AddScoped<IIcLineService, IcLineService>();
        services.AddScoped<IIcCountEntryService, IcCountEntryService>();
        services.AddScoped<IIcImportLineService, IcImportLineService>();
        services.AddScoped<IIcRouteService, IcRouteService>();
        services.AddScoped<IIcTerminalLineService, IcTerminalLineService>();
        services.AddScoped<IWiParameterService, WiParameterService>();
        services.AddScoped<IWoParameterService, WoParameterService>();
        services.AddScoped<IWtParameterService, WtParameterService>();
        services.AddScoped<IGrHeaderService, GrHeaderService>();
        services.AddScoped<IGrLineService, GrLineService>();
        services.AddScoped<IGrRouteService, GrRouteService>();
        services.AddScoped<IGrTerminalLineService, GrTerminalLineService>();
        services.AddScoped<IGrImportDocumentService, GrImportDocumentService>();
        services.AddScoped<IGrImportLineService, GrImportLineService>();
        services.AddScoped<IGrLineSerialService, GrLineSerialService>();
        services.AddScoped<IGoodsReceiptOpenOrderReadRepository, GoodsReceiptOpenOrderReadRepository>();
        services.AddScoped<IGoodReciptFunctionsService, GoodReciptFunctionsService>();
        services.AddScoped<IPHeaderService, PHeaderService>();
        services.AddScoped<IPPackageService, PPackageService>();
        services.AddScoped<IPLineService, PLineService>();
        services.AddScoped<IPackageGoodsReceiptMatcher, PackageGoodsReceiptMatcher>();
        services.AddScoped<IPackageWarehouseTransferMatcher, PackageWarehouseTransferMatcher>();
        services.AddScoped<IPackageWarehouseOutboundMatcher, PackageWarehouseOutboundMatcher>();
        services.AddScoped<IPackageWarehouseInboundMatcher, PackageWarehouseInboundMatcher>();
        services.AddScoped<IPackageShippingMatcher, PackageShippingMatcher>();
        services.AddScoped<IPackageProductionMatcher, PackageProductionMatcher>();
        services.AddScoped<IPackageProductionTransferMatcher, PackageProductionTransferMatcher>();
        services.AddScoped<IPackageSubcontractingIssueMatcher, PackageSubcontractingIssueMatcher>();
        services.AddScoped<IPackageSubcontractingReceiptMatcher, PackageSubcontractingReceiptMatcher>();
        services.AddScoped<IWtFunctionReadRepository, WtFunctionReadRepository>();
        services.AddScoped<IWtFunctionService, WtFunctionService>();
        services.AddScoped<IWtTerminalLineService, WtTerminalLineService>();
        services.AddScoped<IWoFunctionReadRepository, WoFunctionReadRepository>();
        services.AddScoped<IWoFunctionService, WoFunctionService>();
        services.AddScoped<IWoTerminalLineService, WoTerminalLineService>();
        services.AddScoped<IWiFunctionReadRepository, WiFunctionReadRepository>();
        services.AddScoped<IWiFunctionService, WiFunctionService>();
        services.AddScoped<IWiTerminalLineService, WiTerminalLineService>();
        services.AddScoped<IShFunctionReadRepository, ShFunctionReadRepository>();
        services.AddScoped<IShFunctionService, ShFunctionService>();
        services.AddScoped<IShTerminalLineService, ShTerminalLineService>();
        services.AddScoped<IPrFunctionReadRepository, PrFunctionReadRepository>();
        services.AddScoped<IPrFunctionService, PrFunctionService>();
        services.AddScoped<IPrTerminalLineService, PrTerminalLineService>();
        services.AddScoped<IPtFunctionReadRepository, PtFunctionReadRepository>();
        services.AddScoped<IPtFunctionService, PtFunctionService>();
        services.AddScoped<IPtTerminalLineService, PtTerminalLineService>();
        services.AddScoped<ISitFunctionReadRepository, SitFunctionReadRepository>();
        services.AddScoped<ISitFunctionService, SitFunctionService>();
        services.AddScoped<ISitTerminalLineService, SitTerminalLineService>();
        services.AddScoped<ISrtFunctionReadRepository, SrtFunctionReadRepository>();
        services.AddScoped<ISrtFunctionService, SrtFunctionService>();
        services.AddScoped<ISrtTerminalLineService, SrtTerminalLineService>();
        services.AddScoped<IWtHeaderService, WtHeaderService>();
        services.AddScoped<IWtLineService, WtLineService>();
        services.AddScoped<IWtImportLineService, WtImportLineService>();
        services.AddScoped<IWtRouteService, WtRouteService>();
        services.AddScoped<IWtLineSerialService, WtLineSerialService>();
        services.AddScoped<IWoHeaderService, WoHeaderService>();
        services.AddScoped<IWoLineService, WoLineService>();
        services.AddScoped<IWoImportLineService, WoImportLineService>();
        services.AddScoped<IWoRouteService, WoRouteService>();
        services.AddScoped<IWoLineSerialService, WoLineSerialService>();
        services.AddScoped<IWiHeaderService, WiHeaderService>();
        services.AddScoped<IWiLineService, WiLineService>();
        services.AddScoped<IWiImportLineService, WiImportLineService>();
        services.AddScoped<IWiRouteService, WiRouteService>();
        services.AddScoped<IWiLineSerialService, WiLineSerialService>();
        services.AddScoped<IShHeaderService, ShHeaderService>();
        services.AddScoped<IShLineService, ShLineService>();
        services.AddScoped<IShImportLineService, ShImportLineService>();
        services.AddScoped<IShRouteService, ShRouteService>();
        services.AddScoped<IShLineSerialService, ShLineSerialService>();
        services.AddScoped<IPrHeaderService, PrHeaderService>();
        services.AddScoped<IPrOperationService, PrOperationService>();
        services.AddScoped<IPrLineService, PrLineService>();
        services.AddScoped<IPrImportLineService, PrImportLineService>();
        services.AddScoped<IPrRouteService, PrRouteService>();
        services.AddScoped<IPrLineSerialService, PrLineSerialService>();
        services.AddScoped<IPtHeaderService, PtHeaderService>();
        services.AddScoped<IPtLineService, PtLineService>();
        services.AddScoped<IPtImportLineService, PtImportLineService>();
        services.AddScoped<IPtRouteService, PtRouteService>();
        services.AddScoped<IPtLineSerialService, PtLineSerialService>();
        services.AddScoped<ISitHeaderService, SitHeaderService>();
        services.AddScoped<ISitLineService, SitLineService>();
        services.AddScoped<ISitImportLineService, SitImportLineService>();
        services.AddScoped<ISitRouteService, SitRouteService>();
        services.AddScoped<ISitLineSerialService, SitLineSerialService>();
        services.AddScoped<ISrtHeaderService, SrtHeaderService>();
        services.AddScoped<ISrtLineService, SrtLineService>();
        services.AddScoped<ISrtImportLineService, SrtImportLineService>();
        services.AddScoped<ISrtRouteService, SrtRouteService>();
        services.AddScoped<ISrtLineSerialService, SrtLineSerialService>();

        return services;
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Mappings;
using WMS_WEBAPI.Repositories;
using WMS_WEBAPI.Services;
using WMS_WEBAPI.UnitOfWork;
using WMS_WEBAPI.Middleware;
using WMS_WEBAPI.Hubs;
using System.Security.Claims;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.FileProviders;
using System.IO;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Infrastructure.Hangfire;
using WMS_WEBAPI.Options;
using WMS_WEBAPI.Services.Jobs;
using WMS_WEBAPI.Security;

var builder = WebApplication.CreateBuilder(args);

// Load local overrides only in Development.
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
}

var configuredCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origin => origin.Trim().TrimEnd('/'))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray()
    ?? Array.Empty<string>();

if (configuredCorsOrigins.Length == 0)
{
    throw new InvalidOperationException("Cors:AllowedOrigins ayari bos birakilamaz.");
}

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new PermissionAuthorizationConvention());
});
builder.Services.AddMemoryCache();
var dataProtectionKeyPath =
    builder.Configuration["DataProtection:KeyPath"] ??
    Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys");
Directory.CreateDirectory(dataProtectionKeyPath);
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyPath))
    .SetApplicationName("V3RII_WMS");

// SignalR Configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Entity Framework Configuration - Using SQL Server for WMS
builder.Services.AddDbContext<WmsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=wms.db";
    options.UseSqlServer(connectionString);
});

// ERP Database Configuration - Using SQL Server
builder.Services.AddDbContext<ErpDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ErpConnection");
    options.UseSqlServer(connectionString);
});

// AutoMapper Configuration - Automatically discover all mapping profiles in the assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Hangfire Configuration
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.Configure<HangfireMonitoringOptions>(
    builder.Configuration.GetSection(HangfireMonitoringOptions.SectionName));

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new[] { 60, 300, 900 },
    LogEvents = true,
    OnAttemptsExceeded = AttemptsExceededAction.Fail
});

// Add Hangfire server
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "default", "email", "reset-pass-mail", "dead-letter" };
});

// Register Core Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Authentication & Authorization Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAuthorityService, UserAuthorityService>();
builder.Services.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IPermissionDefinitionService, PermissionDefinitionService>();
builder.Services.AddScoped<IPermissionGroupService, PermissionGroupService>();
builder.Services.AddScoped<IUserPermissionGroupService, UserPermissionGroupService>();
builder.Services.AddScoped<IPermissionAccessService, PermissionAccessService>();
builder.Services.AddScoped<ICustomerMirrorService, CustomerMirrorService>();
builder.Services.AddScoped<IStockMirrorService, StockMirrorService>();

// Register Localization Services
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

// Register ERP Services
builder.Services.AddScoped<IErpService, ErpService>();

// Register Platform Services
builder.Services.AddScoped<IPlatformPageGroupService, PlatformPageGroupService>();
builder.Services.AddScoped<ISidebarmenuHeaderService, SidebarmenuHeaderService>();
builder.Services.AddScoped<ISidebarmenuLineService, SidebarmenuLineService>();
builder.Services.AddScoped<IPlatformUserGroupMatchService, PlatformUserGroupMatchService>();

// Register Mobile Services
builder.Services.AddScoped<IMobilePageGroupService, MobilePageGroupService>();
builder.Services.AddScoped<IMobileUserGroupMatchService, MobileUserGroupMatchService>();
builder.Services.AddScoped<IMobilemenuHeaderService, MobilemenuHeaderService>();
builder.Services.AddScoped<IMobilemenuLineService, MobilemenuLineService>();

// Register Good Receipt Services
builder.Services.AddScoped<IGrHeaderService, GrHeaderService>();
builder.Services.AddScoped<IGrHeaderService, GrHeaderService>();
builder.Services.AddScoped<IGrLineService, GrLineService>();
builder.Services.AddScoped<IGrImportDocumentService, GrImportDocumentService>();
builder.Services.AddScoped<IGrImportLineService, GrImportLineService>();
builder.Services.AddScoped<IGrRouteService, GrRouteService>();
builder.Services.AddScoped<IGrLineSerialService, GrLineSerialService>();
builder.Services.AddScoped<IGrTerminalLineService, GrTerminalLineService>();


// Register Warehouse Transfer Services (kept disabled to avoid build issues)
builder.Services.AddScoped<IWtFunctionService,WtFunctionService>();
builder.Services.AddScoped<IWtHeaderService,WtHeaderService>();
builder.Services.AddScoped<IWtLineService, WtLineService>();
builder.Services.AddScoped<IWtImportLineService, WtImportLineService>();
builder.Services.AddScoped<IWtLineSerialService, WtLineSerialService>();
builder.Services.AddScoped<IWtRouteService, WtRouteService>();
builder.Services.AddScoped<IWtTerminalLineService, WtTerminalLineService>();


// Register Product Transfer Services
builder.Services.AddScoped<IPtFunctionService, PtFunctionService>();
builder.Services.AddScoped<IPtHeaderService, PtHeaderService>();
builder.Services.AddScoped<IPtLineService, PtLineService>();
builder.Services.AddScoped<IPtImportLineService, PtImportLineService>();
builder.Services.AddScoped<IPtRouteService, PtRouteService>();
builder.Services.AddScoped<IPtTerminalLineService, PtTerminalLineService>();
builder.Services.AddScoped<IPtLineSerialService, PtLineSerialService>();

// Register Production Services
builder.Services.AddScoped<IPrFunctionService, PrFunctionService>();
builder.Services.AddScoped<IPrHeaderService, PrHeaderService>();
builder.Services.AddScoped<IPrLineService, PrLineService>();
builder.Services.AddScoped<IPrImportLineService, PrImportLineService>();
builder.Services.AddScoped<IPrRouteService, PrRouteService>();
builder.Services.AddScoped<IPrTerminalLineService, PrTerminalLineService>();
builder.Services.AddScoped<IPrLineSerialService, PrLineSerialService>();
builder.Services.AddScoped<IPrHeaderSerialService, PrHeaderSerialService>();

// Register Subcontracting Issue Transfer Services
builder.Services.AddScoped<ISitHeaderService, SitHeaderService>();
builder.Services.AddScoped<ISitLineService, SitLineService>();
builder.Services.AddScoped<ISitImportLineService, SitImportLineService>();
builder.Services.AddScoped<ISitRouteService, SitRouteService>();
builder.Services.AddScoped<ISitTerminalLineService, SitTerminalLineService>();
builder.Services.AddScoped<ISitLineSerialService, SitLineSerialService>();

// Register Subcontracting Receipt Transfer Services
builder.Services.AddScoped<ISrtHeaderService, SrtHeaderService>();
builder.Services.AddScoped<ISrtLineService, SrtLineService>();
builder.Services.AddScoped<ISrtImportLineService, SrtImportLineService>();
builder.Services.AddScoped<ISrtRouteService, SrtRouteService>();
builder.Services.AddScoped<ISrtTerminalLineService, SrtTerminalLineService>();
builder.Services.AddScoped<ISrtFunctionService, SrtFunctionService>();
builder.Services.AddScoped<ISrtLineSerialService, SrtLineSerialService>();

// Register Warehouse Outbound Services
builder.Services.AddScoped<IWoFunctionService, WoFunctionService>();
builder.Services.AddScoped<IWoHeaderService, WoHeaderService>();
builder.Services.AddScoped<IWoLineService, WoLineService>();
builder.Services.AddScoped<IWoImportLineService, WoImportLineService>();
builder.Services.AddScoped<IWoRouteService, WoRouteService>();
builder.Services.AddScoped<IWoTerminalLineService, WoTerminalLineService>();
builder.Services.AddScoped<IWoLineSerialService, WoLineSerialService>();

// Register Warehouse Inbound Services
builder.Services.AddScoped<IWiFunctionService, WiFunctionService>();
builder.Services.AddScoped<IWiHeaderService, WiHeaderService>();
builder.Services.AddScoped<IWiLineService, WiLineService>();
builder.Services.AddScoped<IWiImportLineService, WiImportLineService>();
builder.Services.AddScoped<IWiRouteService, WiRouteService>();
builder.Services.AddScoped<IWiTerminalLineService, WiTerminalLineService>();
builder.Services.AddScoped<IWiLineSerialService, WiLineSerialService>();

// Register Shipping Services
builder.Services.AddScoped<IShHeaderService, ShHeaderService>();
builder.Services.AddScoped<IShFunctionService, ShFunctionService>();
builder.Services.AddScoped<IShLineService, ShLineService>();
builder.Services.AddScoped<IShImportLineService, ShImportLineService>();
builder.Services.AddScoped<IShRouteService, ShRouteService>();
builder.Services.AddScoped<IShTerminalLineService, ShTerminalLineService>();
builder.Services.AddScoped<IShLineSerialService, ShLineSerialService>();

// Register Parameter Services
builder.Services.AddScoped<IGrParameterService, GrParameterService>();
builder.Services.AddScoped<IWtParameterService, WtParameterService>();
builder.Services.AddScoped<IWoParameterService, WoParameterService>();
builder.Services.AddScoped<IWiParameterService, WiParameterService>();
builder.Services.AddScoped<IShParameterService, ShParameterService>();
builder.Services.AddScoped<ISrtParameterService, SrtParameterService>();
builder.Services.AddScoped<ISitParameterService, SitParameterService>();
builder.Services.AddScoped<IPtParameterService, PtParameterService>();
builder.Services.AddScoped<IPrParameterService, PrParameterService>();
builder.Services.AddScoped<IIcParameterService, IcParameterService>();
builder.Services.AddScoped<IPParameterService, PParameterService>();


// Register Function Services
builder.Services.AddScoped<IGoodReciptFunctionsService, GoodReciptFunctionsService>();

// Register Background Job Services
builder.Services.AddScoped<BackgroundJobService>();

// Register Notification Services
builder.Services.AddScoped<INotificationService, NotificationService>();

// Register User Detail Services
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IResetPasswordEmailJob, WMS_WEBAPI.Services.Jobs.ResetPasswordEmailJob>();
builder.Services.AddScoped<IStockSyncJob, StockSyncJob>();
builder.Services.AddScoped<ICustomerSyncJob, CustomerSyncJob>();
builder.Services.AddScoped<IHangfireDeadLetterJob, HangfireDeadLetterJob>();

// Register Package Services
builder.Services.AddScoped<IPHeaderService, PHeaderService>();
builder.Services.AddScoped<IPPackageService, PPackageService>();
builder.Services.AddScoped<IPLineService, PLineService>();

// Add HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

// Localization Configuration
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");



// Request Localization Configuration
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("tr-TR"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("es-ES"),
        new CultureInfo("it-IT")
    };

    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins(configuredCorsOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "WMS_API",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "WMS_Client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:SecretKey"] ?? "WMS_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_2024")),
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            // SignalR Hub için token yakala
            if (!string.IsNullOrEmpty(accessToken) && (
                path.StartsWithSegments("/api/authHub") ||
                path.StartsWithSegments("/authHub") ||
                path.StartsWithSegments("/api/notificationHub") ||
                path.StartsWithSegments("/notificationHub")))
            {
                context.Token = accessToken;
            }
            
            return Task.CompletedTask;
        },
        OnTokenValidated = async context =>
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<WmsDbContext>();
            var claims = context.Principal?.Claims;
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                context.Fail("Token geçersiz: eksik kullanıcı ID");
                return;
            }
            
            // Token'ı hem Authorization header'dan hem de query parameter'dan al (SignalR için)
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var accessToken = context.HttpContext.Request.Query["access_token"].FirstOrDefault();
            
            string? rawToken = null;
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                rawToken = authHeader.Substring("Bearer ".Length).Trim();
            }
            else if (!string.IsNullOrEmpty(accessToken))
            {
                // SignalR için query parameter'dan token al
                rawToken = accessToken;
            }
            
            string? tokenHash = null;
            if (!string.IsNullOrEmpty(rawToken))
            {
                using var sha256Hash = System.Security.Cryptography.SHA256.Create();
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                var builderStr = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builderStr.Append(bytes[i].ToString("x2"));
                }
                tokenHash = builderStr.ToString();
            }

            // Session kontrolü: Token hash ile eşleşen aktif session ara
            var session = await db.UserSessions
                .FirstOrDefaultAsync(s => s.UserId.ToString() == userId 
                    && s.RevokedAt == null 
                    && (tokenHash != null && s.Token == tokenHash));
            
            if (session == null)
            {
                context.Fail("Token geçersiz veya oturum kapandı");
            }
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionCatalog.All.Select(x => x.Code))
    {
        options.AddPolicy(PermissionPolicy.Build(permission), policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(ClaimConstants.SystemAdmin, "true") ||
                context.User.Claims.Any(claim =>
                    string.Equals(claim.Type, ClaimConstants.Permission, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(claim.Value, permission, StringComparison.OrdinalIgnoreCase))));
    }
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WMS API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WmsDbContext>();
    await PermissionCatalogBootstrapper.EnsureSeededAsync(dbContext);
}

GlobalJobFilters.Filters.Add(
    new HangfireJobStateFilter(
        app.Services.GetRequiredService<ILogger<HangfireJobStateFilter>>(),
        app.Services.GetRequiredService<IBackgroundJobClient>(),
        app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<HangfireMonitoringOptions>>(),
        app.Services.GetRequiredService<IServiceScopeFactory>()));

// Migrations / seed are intentionally run out-of-band
// (e.g., dotnet ef database update and explicit seed command).

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WMS API V1");
        c.RoutePrefix = "swagger";
    });
}

// Early CORS middleware: ensures preflight and even error responses have CORS headers.
var allowedCorsOrigins = new HashSet<string>(configuredCorsOrigins, StringComparer.OrdinalIgnoreCase);

app.Use(async (ctx, next) =>
{
    var origin = ctx.Request.Headers["Origin"].ToString();
    if (!string.IsNullOrEmpty(origin) && allowedCorsOrigins.Contains(origin))
    {
        ctx.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        ctx.Response.Headers.Append("Access-Control-Allow-Credentials", "true");

        if (HttpMethods.IsOptions(ctx.Request.Method))
        {
            var requestedHeaders = ctx.Request.Headers["Access-Control-Request-Headers"].ToString();
            var allowHeaders = string.IsNullOrWhiteSpace(requestedHeaders)
                ? "Content-Type, Authorization, X-Branch-Code, Branch-Code, X-Language, x-language, x-branch-code, x-requested-with, x-signalr-user-agent"
                : requestedHeaders;

            ctx.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
            ctx.Response.Headers.Append("Access-Control-Allow-Headers", allowHeaders);
            ctx.Response.Headers.Append("Access-Control-Max-Age", "86400");
            ctx.Response.StatusCode = 204;
            return;
        }
    }

    await next();
});

app.UseExceptionHandler(errApp =>
{
    errApp.Run(async ctx =>
    {
        var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = ctx.RequestServices.GetService<ILogger<Program>>();
        if (ex != null)
        {
            logger?.LogError(ex, "Unhandled exception: {Path}", ctx.Request.Path);
        }

        ctx.Response.StatusCode = 500;
        ctx.Response.ContentType = "application/json";

        var origin = ctx.Request.Headers["Origin"].ToString();
        if (!string.IsNullOrEmpty(origin) && allowedCorsOrigins.Contains(origin))
        {
            if (!ctx.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                ctx.Response.Headers.Append("Access-Control-Allow-Origin", origin);
                ctx.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            }
        }

        var response = ApiResponse<object>.ErrorResult(
            "An error occurred.",
            ex?.Message ?? "An error occurred.",
            500,
            ex?.Message);
        var json = System.Text.Json.JsonSerializer.Serialize(response);
        await ctx.Response.WriteAsync(json);
    });
});

app.UseRouting();
app.UseCors("DevCors");

// Static files for uploaded images - wwwroot folder (default)
app.UseStaticFiles();

// Static files for uploads folder (project root/uploads)
// This serves files from project root/uploads folder at /uploads URL path
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "uploads");
if (Directory.Exists(uploadsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
        RequestPath = "/uploads"
    });
}

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new WMS_WEBAPI.HangfireAuthorizationFilter() }
});

if (!app.Environment.IsDevelopment())
{
    RecurringJob.AddOrUpdate<IStockSyncJob>(
        "erp-stock-sync-job",
        job => job.ExecuteAsync(),
        Cron.MinuteInterval(30));

    RecurringJob.AddOrUpdate<ICustomerSyncJob>(
        "erp-customer-sync-job",
        job => job.ExecuteAsync(),
        Cron.MinuteInterval(30));
}
else
{
    RecurringJob.RemoveIfExists("erp-stock-sync-job");
    RecurringJob.RemoveIfExists("erp-customer-sync-job");
    app.Logger.LogInformation("Skipping recurring ERP sync jobs in Development environment.");
}

// X-Language header'ını okuyacak custom middleware
app.UseMiddleware<LanguageMiddleware>();

// Login branch list must always be reachable (CRM parity for getBranches AllowAnonymous).
// Bypass auth pipeline explicitly to prevent transient startup/session 403s on login screen.
app.Use(async (ctx, next) =>
{
    if (HttpMethods.IsGet(ctx.Request.Method) &&
        ctx.Request.Path.Equals("/api/Erp/getBranches", StringComparison.OrdinalIgnoreCase))
    {
        int? branchNo = null;
        var branchNoRaw = ctx.Request.Query["branchNo"].ToString();
        if (int.TryParse(branchNoRaw, out var parsedBranchNo))
        {
            branchNo = parsedBranchNo;
        }

        var erpService = ctx.RequestServices.GetRequiredService<IErpService>();
        var result = await erpService.GetBranchesAsync(branchNo);

        ctx.Response.StatusCode = result.StatusCode;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(result);
        return;
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Endpoint mapping
// SignalR hubs must be mapped before MapControllers() for proper routing
app.MapHub<AuthHub>("/authHub");
app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();

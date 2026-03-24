using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
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
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using WMS_WEBAPI.Extensions;

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
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "text/json",
        "application/problem+json"
    });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new PermissionAuthorizationConvention());
});
builder.Services.AddMemoryCache();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);

// SignalR Configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

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
                .Where(s => s.UserId.ToString() == userId 
                    && s.RevokedAt == null 
                    && (tokenHash != null && s.Token == tokenHash))
                            .FirstOrDefaultAsync();
            
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

app.UseExceptionHandler();

app.UseRouting();
app.UseCors("DevCors");
app.UseResponseCompression();

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
        "*/30 * * * *");

    RecurringJob.AddOrUpdate<ICustomerSyncJob>(
        "erp-customer-sync-job",
        job => job.ExecuteAsync(),
        "*/30 * * * *");
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

using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using WMS_WEBAPI.Hubs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Middleware;
using WMS_WEBAPI.Options;
using WMS_WEBAPI.Services.Jobs;
using WMS_WEBAPI.Infrastructure.Hangfire;

namespace WMS_WEBAPI.Extensions;

public static class WebApplicationPipelineExtensions
{
    public static void ConfigureHangfireJobStateFilter(this WebApplication app)
    {
        GlobalJobFilters.Filters.Add(
            new HangfireJobStateFilter(
                app.Services.GetRequiredService<ILogger<HangfireJobStateFilter>>(),
                app.Services.GetRequiredService<IBackgroundJobClient>(),
                app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<HangfireMonitoringOptions>>(),
                app.Services.GetRequiredService<IServiceScopeFactory>()));
    }

    public static void ConfigureWmsHttpPipeline(this WebApplication app, IEnumerable<string> configuredCorsOrigins)
    {
        ConfigureSwagger(app);
        UseEarlyCors(app, configuredCorsOrigins);

        app.UseExceptionHandler();
        app.UseRouting();
        app.UseCors("DevCors");
        app.UseResponseCompression();

        // Static files for uploaded images - wwwroot folder (default)
        app.UseStaticFiles();

        // Static files for uploads folder (project root/uploads)
        UseUploadsStaticFiles(app);

        ConfigureHangfire(app);

        // X-Language header'ını okuyacak custom middleware
        app.UseMiddleware<LanguageMiddleware>();

        // Login branch list must always be reachable (CRM parity for getBranches AllowAnonymous).
        // Bypass auth pipeline explicitly to prevent transient startup/session 403s on login screen.
        UseErpLoginBranchBypass(app);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<AuthHub>("/authHub");
        app.MapHub<NotificationHub>("/notificationHub");
        app.MapControllers();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WMS API V1");
            c.RoutePrefix = "swagger";
        });
    }

    private static void UseEarlyCors(WebApplication app, IEnumerable<string> configuredCorsOrigins)
    {
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
    }

    private static void UseUploadsStaticFiles(WebApplication app)
    {
        var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "uploads");
        if (!Directory.Exists(uploadsPath))
        {
            return;
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/uploads"
        });
    }

    private static void ConfigureHangfire(WebApplication app)
    {
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
            return;
        }

        RecurringJob.RemoveIfExists("erp-stock-sync-job");
        RecurringJob.RemoveIfExists("erp-customer-sync-job");
        app.Logger.LogInformation("Skipping recurring ERP sync jobs in Development environment.");
    }

    private static void UseErpLoginBranchBypass(WebApplication app)
    {
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
    }
}


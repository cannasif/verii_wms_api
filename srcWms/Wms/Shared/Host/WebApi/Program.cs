using Hangfire;
using Hangfire.Dashboard;
using System.Text;
using Wms.Application.Common;
using Wms.WebApi.Helpers;
using Wms.WebApi.Extensions;
using Wms.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wms.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

var sharedConfigDirectory = Path.Combine(
    builder.Environment.ContentRootPath,
    "Shared",
    "Host",
    "WebApi",
    "Config");

builder.Configuration
    .AddJsonFile(Path.Combine(sharedConfigDirectory, "appsettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(
        Path.Combine(sharedConfigDirectory, $"appsettings.{builder.Environment.EnvironmentName}.json"),
        optional: true,
        reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var localizationService = context.HttpContext.RequestServices.GetRequiredService<ILocalizationService>();
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(error =>
                string.IsNullOrWhiteSpace(error.ErrorMessage) ? localizationService.GetLocalizedString("ValidationError") : error.ErrorMessage))
            .ToList();

        var message = localizationService.GetLocalizedString("InvalidModelState");
        var exceptionMessage = errors.Count > 0
            ? string.Join(" | ", errors)
            : localizationService.GetLocalizedString("ValidationError");

        var response = new ApiResponse<object?>
        {
            Success = false,
            Message = message,
            ExceptionMessage = exceptionMessage,
            Errors = errors,
            StatusCode = StatusCodes.Status400BadRequest,
            ClassName = "ApiResponse<Object>"
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(static type => BuildSwaggerSchemaId(type));
});
builder.Services.AddAuthorization();
builder.Services.AddPragmaticWebApi(builder.Configuration);

var app = builder.Build();
var swaggerEnabled = builder.Configuration.GetValue("Swagger:Enabled", true);

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new[] { 60, 300, 900 },
    LogEvents = true,
    OnAttemptsExceeded = AttemptsExceededAction.Fail
});
GlobalJobFilters.Filters.Add(
    new HangfireJobStateFilter(
        app.Services.GetRequiredService<ILogger<HangfireJobStateFilter>>(),
        app.Services.GetRequiredService<IBackgroundJobClient>(),
        app.Services.GetRequiredService<IOptions<HangfireMonitoringOptions>>(),
        app.Services.GetRequiredService<IServiceScopeFactory>()));

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PragmaticCors");

app.Use(async (context, next) =>
{
    var branchCode = context.Request.Headers["X-Branch-Code"].FirstOrDefault();
    context.Items["BranchCode"] = BranchCodeDefaults.Normalize(branchCode);
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

static string BuildSwaggerSchemaId(Type type)
{
    if (!type.IsGenericType)
    {
        return SanitizeSwaggerSchemaId(type.FullName ?? type.Name);
    }

    var builder = new StringBuilder();
    builder.Append(type.Name.Split('`')[0]);

    foreach (var argument in type.GetGenericArguments())
    {
        builder.Append('_');
        builder.Append(BuildSwaggerSchemaId(argument));
    }

    return SanitizeSwaggerSchemaId(builder.ToString());
}

static string SanitizeSwaggerSchemaId(string value)
{
    return value
        .Replace('.', '_')
        .Replace('+', '_')
        .Replace('[', '_')
        .Replace(']', '_')
        .Replace(',', '_')
        .Replace('`', '_');
}

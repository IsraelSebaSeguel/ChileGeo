using Microsoft.AspNetCore.Mvc.Filters;

namespace ChileGeo.Api.Security;

/// <summary>Simple API-key authentication (Filter pattern) applied to controllers via [ApiKeyAuth].
/// Rejects requests that do not present a valid X-Api-Key header. Configured in appsettings ("Security:ApiKey").</summary>
public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configuredKey = _configuration["Security:ApiKey"];

        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            // No key configured: authentication disabled (useful for local development).
            await next();
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey) ||
            !string.Equals(providedKey, configuredKey, StringComparison.Ordinal))
        {
            context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult(new { message = "API key inválida o ausente." });
            return;
        }

        await next();
    }
}

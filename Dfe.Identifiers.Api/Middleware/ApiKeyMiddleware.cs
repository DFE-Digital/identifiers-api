using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Domain.Authentication;

namespace Dfe.Identifiers.Api.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
{
    private readonly ILogger<ApiKeyMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task InvokeAsync(HttpContext context, IApiKeyService apiKeyService)
    {
        // Only run API auth on API routes
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await next(context);
        }
        else
        {
            if (!context.Request.Headers.TryGetValue(AuthenticationConstants.APIKEYNAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided.");
                return;
            }

            var user = apiKeyService.Execute(extractedApiKey!);

            if (user is null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
            }
            else
            {
                using (_logger.BeginScope("requester: {requester}", user.UserName))
                {
                    await next(context);
                }
            }
        }
    }
}
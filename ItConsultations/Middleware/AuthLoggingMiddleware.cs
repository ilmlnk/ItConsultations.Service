using ItConsultations.Logger.Services;
using System.Security.Claims;

namespace ItConsultations.Middleware;

public class AuthLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public AuthLoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var userId = "anonymous";
        var userEmail = "anonymous";

        Guard.NotNull(context.User.Identity, nameof(context.User.Identity));
        Guard.True(context.User.Identity.IsAuthenticated, nameof(context.User.Identity.IsAuthenticated));

        userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";
        userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value ?? "unknown";

        await _loggingService.LogAsync(
            $"Request started: {context.Request.Method} {context.Request.Path} by {userEmail}",
            Models.LogLevel.Debug,
            "AuthMiddleware"
        );

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await _loggingService.LogAsync(
                $"Request failed for user {userEmail}: {ex.Message}",
                Models.LogLevel.Error,
                "AuthMiddleware",
                ex
            );
            throw;
        }
        finally
        {
            var duration = DateTime.UtcNow - startTime;
            await _loggingService.LogAsync(
                $"Request completed for user {userEmail} in {duration.TotalMilliseconds}ms - Status: {context.Response.StatusCode}",
                Models.LogLevel.Debug,
                "AuthMiddleware"
            );
        }
    }
}

public static class AuthLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthLoggingMiddleware>();
    }
} 
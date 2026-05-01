using MyMvcApp.Models;

namespace MyMvcApp.Middleware;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleAuthorizationMiddleware> _logger;

    private static readonly (string PathPrefix, string Role)[] ProtectedPaths = new[]
    {
        ("/Products/Create", Roles.Admin),
        ("/Products/Edit",   Roles.Admin),
        ("/Products/Delete", Roles.Admin),
    };

    public RoleAuthorizationMiddleware(RequestDelegate next, ILogger<RoleAuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        var rule = ProtectedPaths.FirstOrDefault(r =>
            path.StartsWith(r.PathPrefix, StringComparison.OrdinalIgnoreCase));

        if (rule.PathPrefix is not null)
        {
            var user = context.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("Unauthenticated access attempt to {Path}", path);
                context.Response.Redirect($"/Account/Login?returnUrl={Uri.EscapeDataString(path)}");
                return;
            }

            if (!user.IsInRole(rule.Role))
            {
                _logger.LogWarning(
                    "User {User} (role {UserRole}) denied access to {Path} (requires {RequiredRole})",
                    user.Identity.Name, GetRole(user), path, rule.Role);
                context.Response.Redirect("/Account/AccessDenied");
                return;
            }
        }

        await _next(context);
    }

    private static string GetRole(System.Security.Claims.ClaimsPrincipal user) =>
        user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "(none)";
}

public static class RoleAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseRoleAuthorization(this IApplicationBuilder app) =>
        app.UseMiddleware<RoleAuthorizationMiddleware>();
}

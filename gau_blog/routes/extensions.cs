using gau_blog.services;

namespace gau_blog.routes;

public static class Extensions
{
    public static RouteGroupBuilder RequireJwtAuthentication(this RouteGroupBuilder group)
    {
        return group.AddEndpointFilter(async (context, next) =>
        {
            var httpContext = context.HttpContext;
            var token = httpContext.Request.Cookies["auth_token"];

            if (string.IsNullOrEmpty(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new { message = "Unauthorized: Missing token" });
                return new ValueTask<object?>(null);
            }

            var jwtService = httpContext.RequestServices.GetRequiredService<JwtService>();
            var principal = jwtService.ValidateToken(token); 

            if (principal == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new { message = "Unauthorized: Invalid or expired token" });
                return new ValueTask<object?>(null);
            }

            httpContext.Items["UserId"] = principal.FindFirst("user_id")?.Value ?? string.Empty;
            httpContext.Items["Permission"] = principal.FindFirst("permission")?.Value ?? string.Empty;
            httpContext.Items["Fullname"] = principal.FindFirst("fullname")?.Value ?? string.Empty;

            return await next(context);
        });
    }
}
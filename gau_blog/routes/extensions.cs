using gau_blog.middlewares;


namespace gau_blog.routes;

public static class Extensions
{
    public static RouteGroupBuilder RequireJwtAuthentication(this RouteGroupBuilder group)
    {
        return group.AddEndpointFilter(async (context, next) =>
        {
            var token = context.HttpContext.Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.HttpContext.Response.WriteAsync("Unauthorized: Token is missing");
                return new ValueTask<object?>(null);
            }

            var jwtMiddleware = context.HttpContext.RequestServices.GetRequiredService<JwtMiddleware>();
            try
            {
                jwtMiddleware.ValidateToken(token);
            }
            catch
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.HttpContext.Response.WriteAsync("Unauthorized: Invalid or expired token");
                return new ValueTask<object?>(null);
            }

            return await next(context); 
        });
    }
}
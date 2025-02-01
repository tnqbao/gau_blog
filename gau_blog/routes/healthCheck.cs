namespace gau_blog.routes;

public class HealthCheckRoutes
{
    public static void MapRoutes(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/health", async (HttpContext context) =>
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Healthy");
        });
    }
    
}
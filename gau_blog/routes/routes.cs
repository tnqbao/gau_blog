using gau_blog.apis;
using gau_blog.middlewares;

namespace gau_blog.routes;

public static class Routes
{
    public static void MapRoutes(WebApplication app)
    {
        var blogApi = app.Services.CreateScope().ServiceProvider.GetRequiredService<BlogApi>();
        app.UseMiddleware<CORSMiddleware>();
        var rootGroup = app.MapGroup("/api/storisy");
        BlogRoutes.MapRoutes(rootGroup, blogApi);
        HealthCheckRoutes.MapRoutes(rootGroup);
    }
}
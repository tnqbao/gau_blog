namespace gau_blog.routes;

public static class Routes
{
    public static void MapRoutes(WebApplication app)
    {
        app.UseMiddleware<CORSMiddleware>();
        var rootGroup = app.MapGroup("/api/storisy");
        BlogRoutes.MapRoutes(rootGroup);
    }
}
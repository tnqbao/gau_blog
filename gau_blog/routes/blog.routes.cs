using Microsoft.AspNetCore.Mvc;

namespace gau_blog.routes;

public class BlogRoutes
{
    public static void RegisterRoutes(IRouteBuilder routes)
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    }
    
    public static void RegisterRoutes(IEndpointRouteBuilder routes)
    {
        routes.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }
    
    
}
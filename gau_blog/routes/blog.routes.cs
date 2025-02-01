using gau_blog.apis;
using gau_blog.routes;


public static class BlogRoutes
{
    public static void MapRoutes(IEndpointRouteBuilder routes, BlogApi blogApi)
    {

        var blogRoutes = routes.MapGroup("/blog");

        blogRoutes.MapGet("/{id}", async (long id) => await blogApi.GetBlogByIdAsync(id));

        var privateRoutes = blogRoutes.MapGroup("/");
        privateRoutes.RequireJwtAuthentication();
        privateRoutes.MapPut("/", async (HttpContext context) => await blogApi.CreateBlogAsync(context));
        privateRoutes.MapDelete("/{id}",
            async (HttpContext context, long id) => await blogApi.DeleteBlogByIdAsync(context, id));
        privateRoutes.MapPut("/{id}", async (HttpContext context,long id) => await blogApi.UpdateBlogAsync(context, id));
    }
}
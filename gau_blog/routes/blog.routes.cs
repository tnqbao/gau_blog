using gau_blog.apis;
using gau_blog.middlewares;
using gau_blog.models;
using gau_blog.routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class BlogRoutes
{
    public static void MapRoutes(IEndpointRouteBuilder routes)
    {
        var blogApi = routes.ServiceProvider.GetRequiredService<BlogApi>();

        var blogRoutes = routes.MapGroup("/blog");

        blogRoutes.MapGet("/{id}", async (long id) => await blogApi.GetBlogByIdAsync(id));
        
        var privateRoutes = blogRoutes.MapGroup("/");
        privateRoutes.RequireJwtAuthentication();
        privateRoutes.MapPut("/", async (long id) => await blogApi.CreateBlogAsync(id));
        privateRoutes.MapDelete("/{id}", async (long id) => await blogApi.DeleteBlogByIdAsync(id));
        privateRoutes.MapPut("/{id}", async (long id) => await blogApi.UpdateBlogAsync(id));
    }
}
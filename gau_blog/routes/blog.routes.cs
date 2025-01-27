using gau_blog.apis;
using gau_blog.models;

public static class BlogRoutes
{
    public static void MapRoutes(WebApplication app)
    {
        var blogApi = app.Services.GetRequiredService<BlogApi>();

        // GET: /blog/{id}
        app.MapGet("/blog/{id}", async (long id) =>
        {
            await blogApi.GetBlogByIdAsync(id);
        });

        // POST: /blog
        app.MapPost("/blog", async (Blog blog) =>
        {
            await blogApi.CreateBlogAsync(blog);
        });
        
        // DELETE: /blog/{id}
        app.MapDelete("/blog/{id}", async (long id) =>
        {
            await blogApi.DeleteBlogByIdAsync(id);
        });
        
        // PUT: /blog/{id}
        app.MapPut("/blog/{id}", async (long id, Blog blog) =>
        {
            blog.Id = id;
            await blogApi.UpdateBlogAsync(blog);
        });
    }
}
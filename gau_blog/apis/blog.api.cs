using gau_blog.models;
using gau_blog.repositories;
using gau_blog.utils;

namespace gau_blog.apis
{
    public class BlogApi
    {
        private readonly BlogRepository _blogRepository;

        public BlogApi(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IResult> GetBlogByIdAsync(long id)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return Results.NotFound(new { message = "Blog not found!" });
            }

            var response = new
            {
                message = "Blog found",
                id = blog.Id,
                title = blog.Title,
                body = blog.Body,
                createAt = blog.CreatedAt,
                upvote = blog.Upvote,
                downvote = blog.Downvote,
            };
            return Results.Ok(response);
        }

        public async Task<IResult> CreateBlogAsync(HttpContext context)
        {
            if (!context.Items.TryGetValue("UserId", out var userIdObj) || !long.TryParse(userIdObj?.ToString(), out long userId))
            {
                return Results.Json(new { message = "User is not authenticated." }, statusCode: StatusCodes.Status401Unauthorized);
            }

            CreateBlogDto? blogDto;
            try
            {
                blogDto = await context.Request.ReadFromJsonAsync<CreateBlogDto>();
            }
            catch
            {
                return Results.BadRequest(new { message = "Invalid JSON format" });
            }

            if (blogDto == null || string.IsNullOrEmpty(blogDto.Title) || string.IsNullOrEmpty(blogDto.Body))
            {
                return Results.BadRequest(new { message = "Title and Body are required" });
            }

            var blog = new Blog
            {
                Title = blogDto.Title,
                Body = blogDto.Body,
                Tags = "",
                Upvote = 0,
                Downvote = 0,
                AuthorId = userId
            };

            try
            {
                var newBlog = await _blogRepository.CreateBlogAsync(blog);
                return Results.Created($"/blogs/{newBlog.Id}", new { message = "Blog created successfully!", blog = newBlog });
            }
            catch (Exception e)
            {
                return Results.Problem($"An error occurred: {e.Message}", statusCode: 500);
            }
        }


        public async Task<IResult> DeleteBlogByIdAsync(HttpContext context, long id)
        {
            if (!context.Items.TryGetValue("UserId", out var userIdObj) || userIdObj is not long userId)
            {
                return Results.Json(new { message = "User is not authenticated." }, statusCode: StatusCodes.Status401Unauthorized);
            }

            var blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return Results.NotFound(new { message = "Blog not found!" });
            }

            if (blog.AuthorId != userId)
            {
                return Results.Json(new { message = "You are not the author of this blog!" }, statusCode: StatusCodes.Status403Forbidden);
            }

            var isDeleted = await _blogRepository.DeleteBlogByIdAsync(id);
            if (isDeleted == null)
            {
                return Results.Problem("An error occurred while trying to delete the blog.");
            }

            return Results.Ok(new { message = "Blog deleted successfully!" });
        }

        public async Task<IResult> UpdateBlogAsync(HttpContext context, long id)
        {
            var existingBlog = await _blogRepository.GetBlogByIdAsync(id);
            if (existingBlog == null)
            {
                return Results.NotFound(new { message = "Blog not found!" });
            }

            // existingBlog.Title = blog.Title;
            // existingBlog.Body = blog.Body;
            // existingBlog.Tags = blog.Tags;
            // existingBlog.Upvote = blog.Upvote;
            // existingBlog.Downvote = blog.Downvote;

            try
            {
                var updatedBlog = await _blogRepository.UpdateBlogAsync(existingBlog);
                return Results.Ok(updatedBlog);
            }
            catch (Exception e)
            {
                return Results.Problem($"An error occurred: {e.Message}", statusCode: 500);
            }
        }
    }
}
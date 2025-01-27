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

        public async Task<IResult> CreateBlogAsync(Blog blog)
        {
            if (string.IsNullOrEmpty(blog.Title) || string.IsNullOrEmpty(blog.Body))
            {
                return Results.BadRequest(new { message = "Title and Content are required" });
            }

            try
            {
                var newBlog = await _blogRepository.CreateBlogAsync(blog);
                return Results.Created($"/blogs/{newBlog.Id}", newBlog);
            }
            catch (Exception e)
            {
                return Results.Problem($"An error occurred: {e.Message}", statusCode: 500);
            }
        }

        public async Task<IResult> DeleteBlogByIdAsync(long id)
        {
            var blog = await _blogRepository.DeleteBlogByIdAsync(id);
            if (blog == null)
            {
                return Results.NotFound(new { message = "Blog not found!" });
            }

            return Results.Ok(new { message = "Blog deleted!" });
        }
        
        public async Task<IResult> UpdateBlogAsync(Blog blog)
        {
            var existingBlog = await _blogRepository.GetBlogByIdAsync(blog.Id);
            if (existingBlog == null)
            {
                return Results.NotFound(new { message = "Blog not found!" });
            }

            existingBlog.Title = blog.Title;
            existingBlog.Body = blog.Body;
            existingBlog.Tags = blog.Tags;
            existingBlog.Upvote = blog.Upvote;
            existingBlog.Downvote = blog.Downvote;

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
using gau_blog.config;
using gau_blog.models;
using Microsoft.EntityFrameworkCore;

namespace gau_blog.repositories;

public class BlogRepository
{
    private readonly ApplicationDbContext _context;

    public BlogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Blog> CreateBlogAsync(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog> GetBlogByIdAsync(long id)
    {
        var blog = await _context.Blogs.AsNoTracking().Where(b => b.Id == id).FirstOrDefaultAsync();
        
        if (blog == null)
        {
            return null;
        }
        return blog;
    }

    public async Task<bool> DeleteBlogByIdAsync(long id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null)
        {
            return false;
        }

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Blog> UpdateBlogAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
}
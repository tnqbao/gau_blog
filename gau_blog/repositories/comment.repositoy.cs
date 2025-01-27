using gau_blog.config;
using gau_blog.models;
namespace gau_blog.repositories;

public class CommentRepository
{
    private readonly ApplicationDbContext _context;
    
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
    
    public async Task<Comment> GetCommentByIdAsync(long id)
    {
        return await _context.Comments.FindAsync(id);
    }
    
    public async Task<Comment> DeleteCommentByIdAsync(long id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return null;
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
    
    public async Task<Comment> UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
}
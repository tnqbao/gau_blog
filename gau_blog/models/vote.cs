using System.ComponentModel.DataAnnotations.Schema;

namespace gau_blog.models;

public class Vote
{
    [ForeignKey("BlogId")]
    private long BlogId { get; set; }
    
    private bool Status { get; set; }
    
    private DateTime CreatedAt { get; set; }
    
    public Vote()
    {
    }
    
    public Vote(long blogId, bool status, DateTime createdAt)
    {
        BlogId = blogId;
        Status = status;
        CreatedAt = createdAt;
    }
}
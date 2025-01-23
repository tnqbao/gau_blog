using System.ComponentModel.DataAnnotations.Schema;

namespace gau_blog.api;

public class vote_controllers
{
    [ForeignKey("BlogId")]
    private string BlogId { get; set; }
    
    private bool Status { get; set; }
    private DateTime CreatedAt { get; set; }
}
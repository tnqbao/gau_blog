using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gau_blog.models
{
    public class Comment
    {
        [Key] public long Id { get; set; }

        [Required] [MaxLength(1000)] public string Content { get; set; }

        [Required] public long BlogId { get; set; }

        [ForeignKey("BlogId")] public Blog Blog { get; set; }

        [Required] public long AuthorId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Comment() {}

        public Comment(long id, string content, long blogId, long authorId, DateTime createdAt)
        {
            Id = id;
            Content = content;
            BlogId = blogId;
            AuthorId = authorId;
            CreatedAt = createdAt;
        }
        
    }
}
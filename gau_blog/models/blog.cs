using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace gau_blog.models
{
    public class Blog
    {
        [Key] public long Id { get; set; }

        [Required] [MaxLength(200)] public string Title { get; set; }

        [Required] public string Body { get; set; }

        public long AuthorId { get; set; }

        [DefaultValue(0)] public int Upvote { get; set; }
        [DefaultValue(0)] public int Downvote { get; set; }

        [MaxLength(500)] public string Tags { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; } = new List<Comment>();
        
        public Blog()
        {
        }

        public Blog(string title, string body, long authorId, int upvote, int downvote, string tags, DateTime createdAt)
        {
            Title = title;
            this.Body = body;
        }
    }
}
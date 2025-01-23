using gau_blog.models;
using Microsoft.EntityFrameworkCore;

namespace gau_blog.config
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.BlogId)
                .HasDatabaseName("IX_Comment_BlogId");

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.AuthorId)
                .HasDatabaseName("IX_Comment_AuthorId");

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}
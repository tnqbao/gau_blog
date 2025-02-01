using gau_blog.models;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace gau_blog.config
{
    public static class Database
    {
        /// <summary>
        /// Database configuration and load from environment variables
        /// </summary>
        /// <param name="services">Service collection</param>
        public static void ConfigureDatabase(this IServiceCollection services)
        {
            Env.Load();

            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "password";
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "database";

            var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database}";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }

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
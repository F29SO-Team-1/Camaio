using Microsoft.EntityFrameworkCore;

namespace Login.Models
{
    public class PostContent : DbContext
    {
        public PostContent(DbContextOptions<PostContent> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
    }
}

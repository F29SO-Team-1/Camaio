using Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.Data
{
    public class TagContext : DbContext
    {
        public TagContext(DbContextOptions<TagContext> options)
            : base(options)
        {
        }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<Channel>()
                .ToTable("Channel")
                .HasMany(channel => channel.Tags)
                .WithOne(tag => tag.Channel);
            modelBuilder.Entity<Thread>()
                .ToTable("Thread")
                .HasMany(thread => thread.Tags)
                .WithOne(tag => tag.Thread);
        }
    }
}

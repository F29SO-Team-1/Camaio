using Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.Data
{
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> options)
            : base(options)
        {
        }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelMember> ChannelMember { get; set; }
        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>().ToTable("Channel");
            modelBuilder.Entity<ChannelMember>().ToTable("ChannelMember");
            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<ChannelMember>()
                .HasOne(cm => cm.Channel)
                .WithMany(c => c.ChannelMembers);
            modelBuilder.Entity<Album>()
                .HasOne(album => album.Channel)
                .WithMany(channel => channel.Albums);
            modelBuilder.Entity<Album>()
                .HasMany(album => album.Threads)
                .WithOne(thread => thread.Album);
        }
    }
}
